using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Registrar.Api.Auth;
using Registrar.Api.Models.Request;
using Registrar.Api.Models.Response;

namespace Registrar.Api.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : ApiController
    {
        private RegiSignInManager _signInManager;
        private RegiUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(RegiUserManager userManager, RegiSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public RegiSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.Current.GetOwinContext().Get<RegiSignInManager>(); }
            private set { _signInManager = value; }
        }

        public RegiUserManager UserManager
        {
            get { return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<RegiUserManager>(); }
            private set { _userManager = value; }
        }

        [HttpGet]
        [Route("details/{userId:int}")]
        [Authorize]
        public async Task<IHttpActionResult> Details(int userId)
        {
            if (User.Identity.GetUserId<int>() != userId)
                return Unauthorized();

            var details = await UserManager.FindByIdAsync(userId);
            var res = new SingleRegiUserResponse(details);
            return Ok(res);
        }

        // POST: /Account/VerifyCode
        [HttpPost]
        [Route("verifyCode")]
        public async Task<IHttpActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result =
                await
                    SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, true);
            switch (result)
            {
                case SignInStatus.Success:
                    return Ok();
                case SignInStatus.LockedOut:
                    return StatusCode(HttpStatusCode.Forbidden);
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return BadRequest(ModelState);
            }
        }

        // POST: /Account/Register
        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> Register(CreateRegiUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new RegiAuthUser {UserName = model.Email, Email = model.Email};
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, false, false);

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                return Ok();
            }
            AddErrors(result);

            // If we got this far, something failed, redisplay form
            return BadRequest(ModelState);
        }

        // GET: /Account/ConfirmEmail
        public async Task<IHttpActionResult> ConfirmEmail(int userId, string code)
        {
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
                return BadRequest();

            return Ok();
        }


        // POST: /Account/ForgotPassword
        [HttpPost]
        [Route("forgotPassword")]
        public async Task<IHttpActionResult> ForgotPassword(ForgotRegiPassword model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await UserManager.FindByNameAsync(model.Email);
            if ((user == null) || !await UserManager.IsEmailConfirmedAsync(user.Id))
                return Ok();

            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link
            // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
            // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
            // return RedirectToAction("ForgotPasswordConfirmation", "Account");

            // If we got this far, something failed, redisplay form
            return BadRequest(ModelState);
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [Route("resetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetRegiPassword model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
                return StatusCode(HttpStatusCode.Forbidden);
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
                return Ok();
            AddErrors(result);
            return BadRequest();
        }

        //// GET: /Account/SendCode
        //[System.Web.Http.HttpGet]
        //[System.Web.Http.Route("sendCode")]
        //public async Task<IHttpActionResult> SendCode()
        //{
        //    var userId = await SignInManager.GetVerifiedUserIdAsync();
        //    if (userId == null)
        //        return StatusCode(HttpStatusCode.Forbidden);
        //    var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
        //    var factorOptions =
        //        userFactors.Select(purpose => new System.Web.Mvc.SelectListItem {Text = purpose, Value = purpose}).ToList();
        //    return
        //        Ok(new SingleRegiCodeResponse
        //        {
        //            Providers = factorOptions
        //        });
        //}

        // POST: /Account/SendCode
        [HttpPost]
        [Route("sendCode")]
        public async Task<IHttpActionResult> SendCode(RegiCode model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
                return StatusCode(HttpStatusCode.Forbidden);
            return Ok(new {Provider = model.SelectedProvider});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }

        #endregion
    }
}