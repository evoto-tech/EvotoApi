using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Common.Models;
using EvotoApi.Areas.Management.Connections;
using Registrar.Models.Request;
using EvotoApi.Areas.ManagementApi.Models.Response;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EvotoApi.Areas.Management.Controllers
{
    [RoutePrefix("regi")]
    public class RegistrarConnectionController : ApiController
    {
        [Route("users/list")]
        // TODO: [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> List()
        {
            try
            {
                var users = await RegistrarConnection.GetRegistrarUsers();
                return Ok(users);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("users/detail/{userId:int}")]
        // TODO: [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> Details(int userId)
        {
            try
            {
                var user = await RegistrarConnection.GetRegistrarUser(userId);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("account/register")]
        // TODO: [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> Register(CreateRegiUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await RegistrarConnection.CreateRegistrarUser(model);
                return Ok(user);
            }
            catch (Exception e)
            {
                // TODO: better error handling
                if ((int) e.Data["status"] != 400)
                {
                    return Json(e.Data["content"]);
                }
                return Json(new
                {
                    errors = e.Message
                });
            }
        }

        [Route("settings/custom-fields")]
        // TODO: [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> SettingsCustomFieldsGet()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var fields = await RegistrarConnection.GetCustomFields();
                return Ok(fields);
            }
            catch (Exception e)
            {
                // TODO: better error handling
                if (e.Data.Contains("status") && (int)e.Data["status"] != 400)
                {
                    return Json(e.Data["content"]);
                }
                return Json(new
                {
                    errors = e.Message
                });
            }
        }

        [Route("settings/custom-fields")]
        // TODO: [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> SettingsCustomFieldsPost(IList<CreateCustomUserFieldModel> model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var fields = await RegistrarConnection.UpdateCustomFields(model);
                return Ok(fields);
            }
            catch (Exception e)
            {
                // TODO: better error handling
                if (e.Data.Contains("status") && (int)e.Data["status"] != 400)
                {
                    return Json(e.Data["content"]);
                }
                return Json(new
                {
                    errors = e.Message
                });
            }
        }

        [Route("settings/list")]
        // TODO: [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> ListSettings()
        {
            try
            {
                var settings = await RegistrarConnection.ListRegistrarSettings();
                return Json(settings);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("error", e);
                return BadRequest();
            }
        }

        [Route("settings")]
        // TODO: [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateSetting(UpdateRegiSetting model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var setting = await RegistrarConnection.UpdateRegistrarSettings(model);
                return Json(setting);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("error", e);
                return BadRequest();
            }
        }
    }
}