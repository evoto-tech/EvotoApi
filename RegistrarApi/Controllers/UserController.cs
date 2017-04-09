using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Common.Exceptions;
using Registrar.Database.Interfaces;
using Registrar.Models;
using Registrar.Models.Request;
using Registrar.Models.Response;

namespace Registrar.Api.Controllers
{
    [RoutePrefix("users")]
    public class UserController : ApiController
    {
        private readonly IRegiUserFieldsStore _fieldStore;
        private readonly IRegiUserStore _store;

        public UserController(IRegiUserStore userStore, IRegiUserFieldsStore fieldStore)
        {
            _store = userStore;
            _fieldStore = fieldStore;
        }

        [HttpGet]
        [Route("list")]
        [ApiKeyAuth]
        public async Task<IHttpActionResult> List()
        {
            try
            {
                var details = await _store.GetUsers();
                var res = details.Select(v => new SingleRegiUserResponse(v));
                return Ok(res);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("details/{userId:int}")]
        [ApiKeyAuth]
        public async Task<IHttpActionResult> Details(int userId)
        {
            try
            {
                var details = await _store.GetUserById(userId);
                var res = new SingleRegiUserResponse(details);
                return Ok(res);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
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
            {
                await _fieldStore.DeleteCustomUserField(d);
            }

            foreach (var u in update)
            {
                await _fieldStore.UpdateCustomUserField(u);
            }

            foreach (var c in create)
            {
                await _fieldStore.CreateCustomUserField(c);
            }

            // Update User View with custom fields (columns)
            await _fieldStore.UpdateUserView();

            // Return the updated fields
            var fields = await _fieldStore.GetCustomUserFields();
            return Ok(fields.Select(f => new SingleCustomUserFieldResponse(f)));
        }
    }
}