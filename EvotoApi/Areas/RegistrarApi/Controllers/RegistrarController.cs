using System.Web.Http;

namespace EvotoApi.Areas.RegistrarApi.Controllers
{
    [RoutePrefix("regi")]
    public class RegistrarController : ApiController
    {
        /// <summary>
        /// Registrar Index
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("", Name = "RegistrarIndex")]
        public IHttpActionResult Index()
        {
            return Ok("Test");
        }
    }
}