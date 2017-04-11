using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Common;
using Common.Exceptions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Registrar.Api.Auth;
using Registrar.Database.Interfaces;
using Registrar.Models;
using Registrar.Models.Exceptions;
using Registrar.Models.Request;
using Registrar.Models.Response;

namespace Registrar.Api.Controllers
{
    [RoutePrefix("users")]
    public class UserController : ApiController
    {
        private readonly IRegiUserFieldsStore _fieldStore;
        private readonly IRegiUserStore _store;

        private RegiUserManager _userManager;

        public UserController(IRegiUserStore userStore, IRegiUserFieldsStore fieldStore)
        {
            _store = userStore;
            _fieldStore = fieldStore;
        }

        public RegiUserManager UserManager
            => _userManager ?? (_userManager = HttpContext.Current.GetOwinContext().Get<RegiUserManager>());

        [HttpGet]
        [ApiKeyAuth]
        public async Task<IHttpActionResult> List()
        {
            var details = await _store.GetUsers();
            var res = details.Select(v => new SingleRegiUserResponse(v));
            return Ok(res);
        }

        [HttpGet]
        [Route("{userId:int}")]
        [ApiKeyAuth]
        public async Task<IHttpActionResult> Details(int userId)
        {
            var details = await UserManager.FindByIdAsync(userId);

            if (details == null)
                return NotFound();

            var res = new SingleRegiUserResponse(details);
            return Ok(res);
        }

        [HttpGet]
        [Route("email")]
        [ApiKeyAuth]
        public async Task<IHttpActionResult> Details(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return NotFound();

            var info = await UserManager.FindByEmailAsync(email);
            if (info == null)
                return NotFound();

            return Ok(new SingleRegiUserResponse(info));
        }

        [HttpPost]
        [Route("customFields/update")]
        [ApiKeyAuth]
        public async Task<IHttpActionResult> UpdateCustomFields(IList<CreateCustomUserFieldModel> models)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (models == null)
                return BadRequest();

            // Read existing fields from database
            var existingFields = await _fieldStore.GetCustomUserFields();

            // Store lists for database interraction
            var update = new List<CustomUserField>();
            var create = new List<CustomUserField>();

            var names = new List<string>();
            var errors = new List<string>();

            foreach (var model in models)
            {
                var validation = new CustomUserValidation(model.Validation);

                // Ensure the field names are unique
                if (names.Contains(model.Name))
                {
                    var errMsg = $"Duplicate Field name: {model.Name}";
                    // Avoid duplicate errors if more than two fields share a name
                    if (!errors.Contains(errMsg))
                        errors.Add(errMsg);
                }
                else
                    names.Add(model.Name);

                // Creating a new field
                if (model.Id == 0)
                {
                    var field = CustomUserField.GetFieldForType(model.Type);
                    field.Name = model.Name;
                    field.Type = model.Type;
                    field.Required = model.Required;
                    if (field.SetValidationProperties(validation))
                        create.Add(field);
                    else
                        errors.Add($"Invalid Field Valididation for {model.Name}");
                }
                // Edit an existing field
                else
                {
                    var field = existingFields.SingleOrDefault(f => f.Id == model.Id);
                    if (field == null)
                    {
                        errors.Add($"Cannot find existing field, ID: {model.Id}");
                        continue;
                    }

                    if (field.Type != model.Type)
                    {
                        errors.Add($"Cannot change type of {field.Name} ({field.Id})");
                        continue;
                    }

                    // Update editable properties
                    field.Name = model.Name;
                    field.Required = model.Required;

                    if (field.SetValidationProperties(validation))
                        update.Add(field);
                    else
                        errors.Add($"Invalid Field Valididation for {model.Name}");
                }
            }

            // Error reading fields, return invalid
            if (errors.Any())
            {
                foreach (var error in errors)
                    ModelState.AddModelError("", error);
                return BadRequest(ModelState);
            }

            // Any fields omitted must have been deleted
            var delete = existingFields.Where(f => !create.Contains(f)).Where(f => !update.Contains(f)).ToList();

            // Sanity checks
            if (create.Count + update.Count != models.Count)
                return InternalServerError();
            if (update.Count + delete.Count != existingFields.Count)
                return InternalServerError();

            // Run synchronously to avoid any uniqueness conflicts and race conditions
            foreach (var d in delete)
                await _fieldStore.DeleteCustomUserField(d);

            foreach (var u in update)
                await _fieldStore.UpdateCustomUserField(u);

            foreach (var c in create)
                await _fieldStore.CreateCustomUserField(c);

            // Update User View with custom fields (columns)
            await _fieldStore.UpdateUserView();

            return Ok();
        }

        [HttpPost]
        [ApiKeyAuth]
        public async Task<IHttpActionResult> Create(CreateRegiUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate custom fields
            var fields = await _fieldStore.GetCustomUserFields();
            var errors = AccountController.ValidateCustomUserFields(model.CustomFields, fields);

            if (errors.Any())
            {
                AddErrors(errors);
                return BadRequest(ModelState);
            }

            // Create account
            var user = new RegiAuthUser {UserName = model.Email, Email = model.Email};
            var result = await UserManager.CreateAsync(user, model.Password);

            // Any errors in UserManager (such as duplicate email or insufficient password strength)
            if (!result.Succeeded)
            {
                AddErrors(result);
                return BadRequest(ModelState);
            }

            // Get created account
            var userModel = await UserManager.FindByEmailAsync(model.Email);

            // Store custom user data
            var fieldsTasks = fields.Where(f => model.CustomFields.Any(m => m.Name == f.Name))
                .Select(field => new CustomUserValue
                {
                    FieldId = field.Id,
                    Value = model.CustomFields.Single(f => f.Name == field.Name).Value
                }).Select(value => _fieldStore.AddFieldValueForUser(userModel, value));
            await Task.WhenAll(fieldsTasks);

            // Send an email confirmation code
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(userModel.Id);
            var body = EmailContentWriter.ConfirmEmail(user.Email, code);
            try
            {
                await UserManager.SendEmailAsync(userModel.Id, EmailContentWriter.ConfirmEmailSubject, body);
            }
            catch (CouldNotSendEmailException)
            {
                return BadRequest("Could not send email");
            }

            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        [ApiKeyAuth]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var user = await UserManager.FindByIdAsync(id);
            await UserManager.DeleteAsync(user);

            return Ok();
        }

        [HttpPost]
        [Route("changePassword")]
        [ApiKeyAuth]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await UserManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound();

            var passwordValid = await UserManager.PasswordValidator.ValidateAsync(model.Password);
            if (!passwordValid.Succeeded)
            {
                AddErrors(passwordValid);
                return BadRequest(ModelState);
            }

            user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);
            var updated = await UserManager.UpdateAsync(user);
            if (!updated.Succeeded)
            {
                AddErrors(updated);
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpPost]
        [Route("{id:int}/confirmEmail")]
        [ApiKeyAuth]
        public async Task<IHttpActionResult> ConfirmEmail(int id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            user.EmailConfirmed = true;
            var updated = await UserManager.UpdateAsync(user);
            if (!updated.Succeeded)
            {
                AddErrors(updated);
                return BadRequest(ModelState);
            }

            return Ok();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            AddErrors(result.Errors);
        }

        private void AddErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
                ModelState.AddModelError("", error);
        }

        #endregion
    }
}