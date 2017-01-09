using System.Web.Mvc;

namespace EvotoApi.Areas.Management.Controllers
{
    [RouteArea("Management")]
    public class ReactController : Controller
    {
        [Route(Name = "reactIndex")]
        public ActionResult Index()
        {
            return View();
        }
    }
}