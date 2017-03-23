using System;
using System.Threading.Tasks;
using System.Web.Http;
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
    }
}