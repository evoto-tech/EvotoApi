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
                var registrarConnection = RegistrarConnection.GetRegistrarUsers();
                return Json(registrarConnection);
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
                var registrarConnection = RegistrarConnection.GetRegistrarUsers();
                return Json(registrarConnection);
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