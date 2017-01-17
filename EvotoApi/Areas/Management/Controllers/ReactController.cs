using System.Web.Mvc;

namespace EvotoApi.Areas.Management.Controllers
{
    public class ReactController : Controller
    {
        [Route(Name = "reactIndex")]
        public ActionResult Index()
        {
            return View();
        }
    }
}