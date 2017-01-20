using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Registrar.Api.Auth;
using Registrar.Api.Models.Request;
using Registrar.Api.Models.Response;

namespace Registrar.Api.Controllers
{
    [System.Web.Http.RoutePrefix("account")]
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

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("login")]
        public async Task<IHttpActionResult> Login(LoginRegiUser model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, false, true);
            switch (result)
            {
                case SignInStatus.Success:
                    return Ok();
                case SignInStatus.LockedOut:
                    return Unauthorized();
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new {ReturnUrl = returnUrl, model.RememberMe});
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return BadRequest(ModelState);
            }
        }

        // POST: /Account/VerifyCode
        [System.Web.Http.HttpPost]
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
                    return Unauthorized();
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return BadRequest(ModelState);
            }
        }

        // POST: /Account/Register
        [System.Web.Http.HttpPost]
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

        //// GET: /Account/ConfirmEmail
        //public async Task<IHttpActionResult> ConfirmEmail(string userId, string code)
        //{
        //    if ((userId == null) || (code == null))
        //        return View("Error");
        //    var result = await UserManager.ConfirmEmailAsync(userId, code);
        //    return View(result.Succeeded ? "ConfirmEmail" : "Error");
        //}


        // POST: /Account/ForgotPassword
        [System.Web.Http.HttpPost]
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
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> ResetPassword(ResetRegiPassword model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
                return Unauthorized();
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
                return Ok();
            AddErrors(result);
            return BadRequest();
        }

        // GET: /Account/SendCode
        public async Task<IHttpActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
                return Unauthorized();
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions =
                userFactors.Select(purpose => new SelectListItem {Text = purpose, Value = purpose}).ToList();
            return
                Ok(new SingleRegiCodeResponse
                {
                    Providers = factorOptions,
                    ReturnUrl = returnUrl,
                    RememberMe = rememberMe
                });
        }

        // POST: /Account/SendCode
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> SendCode(RegiCode model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
                return Unauthorized();
            return Ok(new {Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe});
        }

        // POST: /Account/LogOff
        [System.Web.Http.HttpPost]
        [System.Web.Http.Authorize]
        public IHttpActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return Ok();
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

        // Used for XSRF protection when adding external logins
        private const string XSRF_KEY = "XsrfId";

        private static IAuthenticationManager AuthenticationManager
            => HttpContext.Current.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties {RedirectUri = RedirectUri};
                if (UserId != null)
                    properties.Dictionary[XSRF_KEY] = UserId;
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}