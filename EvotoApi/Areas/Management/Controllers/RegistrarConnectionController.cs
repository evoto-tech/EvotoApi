using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Common.Models;
using EvotoApi.Areas.Management.Connections;
using EvotoApi.Areas.ManagementApi.Models.Response;

namespace EvotoApi.Areas.Management.Controllers
{
    [RoutePrefix("regi")]
    public class RegistrarConnectionController : ApiController
    {
        [Route("users/list")]
        //[Authorize]
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
        //[Authorize]
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
        //[Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> Register(CreateRegiUser model)
        {
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
        //[Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> SettingsCustomFields()
        {
            try
            {
                var fields = await RegistrarConnection.UpdateCustomFields();
                return Json(fields);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    errors = e.Message
                });
            }
        }
    }
}