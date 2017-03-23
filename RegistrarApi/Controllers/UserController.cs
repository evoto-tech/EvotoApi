using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Common.Exceptions;
using Common.Models;
using Registrar.Api.Models.Request;
using Registrar.Api.Models.Response;
using Registrar.Database.Interfaces;

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

        /// <summary>
        ///     Public endpoint. Gets currently defined custom user metadata
        /// </summary>
        [HttpGet]
        [Route("customFields")]
        public async Task<IHttpActionResult> GetCustomFields()
        {
            var fields = await _fieldStore.GetCustomUserFields();
            return Ok(fields.Select(f => new SingleCustomUserFieldResponse(f)));
        }

        [HttpPost]
        [Route("customFields/update")]
        [ApiKeyAuth]
        public async Task<IHttpActionResult> UpdateCustomFields(IList<CreateCustomUserFieldModel> models)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingFields = await _fieldStore.GetCustomUserFields();
            var update = new List<CustomUserField>();
            var create = new List<CustomUserField>();
            var errors = new List<string>();

            foreach (var model in models)
            {
                if (model.Id == 0)
                {
                    var field = CustomUserField.GetFieldForType(model.Type);
                    field.Name = model.Name;
                    field.Type = model.Type;
                    field.Required = model.Required;

                    create.Add(field);
                }
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

                    field.SetValidationProperties(model.Validation);

                    update.Add(field);
                }
            }

            if (errors.Any())
            {
                foreach (var error in errors)
                    ModelState.AddModelError("", error);
                return BadRequest(ModelState);
            }

            var delete = existingFields.Where(f => !create.Contains(f)).Where(f => !update.Contains(f)).ToList();

            // Sanity checks
            if (create.Count + update.Count != models.Count)
                return InternalServerError();
            if (update.Count + delete.Count != existingFields.Count)
                return InternalServerError();

            // Here goes
            var tasks = new List<Task>();
            tasks.AddRange(create.Select(c => _fieldStore.CreateCustomUserField(c)).ToList());
            tasks.AddRange(update.Select(u => _fieldStore.UpdateCustomUserField(u)).ToList());
            tasks.AddRange(delete.Select(d => _fieldStore.DeleteCustomUserField(d)).ToList());

            await Task.WhenAll(tasks);

            return Ok();
        }
    }
}