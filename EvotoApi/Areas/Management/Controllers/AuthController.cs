using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using EvotoApi.Areas.Management.Models.Request;
using EvotoApi.Auth;
using Microsoft.AspNet.Identity.Owin;

namespace EvotoApi.Areas.Management.Controllers
{
    [RoutePrefix("mana/auth")]
    public class AuthController : ApiController
    {
        [Route("login")]
        [HttpPost]
        public async Task<IHttpActionResult> Login(LoginManaUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sm = HttpContext.Current.GetOwinContext().Get<ManaSignInManager>();
            var status = await sm.PasswordSignInAsync(model.Email, model.Password, true, true);

            if (status == SignInStatus.Success)
            {
                return Ok();
            }
            // TODO: Handle lockout/2FA
            return Unauthorized();
        }
    }
}