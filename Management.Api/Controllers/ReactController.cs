using System.Web.Mvc;

namespace EvotoApi.Controllers
{
    public class ReactController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View("Index");
        }
    }
}