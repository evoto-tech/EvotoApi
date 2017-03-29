using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Common.Models;
using EvotoApi.Areas.Management.Connections;

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
                return Json(users);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    errors = e.Message
                });
            }
        }

        [Route("users/detail/{userId:int}")]
        //[Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> Details()
        {
            try
            {
                var users = await RegistrarConnection.GetRegistrarUsers();
                return Json(users);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    errors = e.Message
                });
            }
        }

        [Route("account/register")]
        //[Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> Register(CreateRegiUser user)
        {
            try
            {
                var users = await RegistrarConnection.CreateRegistrarUser(user);
                return Json(users);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Data["status"]);
                Debug.WriteLine(e.Data["content"]);
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
    }
}