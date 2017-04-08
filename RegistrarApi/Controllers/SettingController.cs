using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Common.Exceptions;
using Registrar.Database.Interfaces;
using Registrar.Models.Request;

namespace Registrar.Api.Controllers
{
    [RoutePrefix("settings")]
    public class SettingController : ApiController
    {
        private readonly IRegiSettingStore _store;

        public SettingController(IRegiSettingStore store)
        {
            _store = store;
        }

        [HttpPost]
        [Route("")]
        [ApiKeyAuth]
        public async Task<IHttpActionResult> Post(UpdateRegiSetting setting)
        {
            try
            {
                var updatedSetting = await _store.UpdateSetting(setting);
                return Json(updatedSetting);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("list")]
        [ApiKeyAuth]
        public async Task<IHttpActionResult> List()
        {
            try
            {
                var settings = await _store.ListSettings();
                return Json(settings);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }
    }
}