using System.Web.Mvc;

namespace EvotoApi.Areas.Management.Controllers
{
    public class ReactController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View("Index");
        }
    }
}