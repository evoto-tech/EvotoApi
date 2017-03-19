﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Common.Exceptions;
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
                // Get created account
                var userModel = await UserManager.FindByEmailAsync(model.Email);

                // Send an email with this link
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(userModel.Id);
                var uri = GetUri("confirmemail", model.Email, code);
                try
                {
                    await UserManager.SendEmailAsync(userModel.Id, "Confirm your account",
                        $"Email confirmation code: {code}<br /><br />Alternatively, click <a href=\"{uri}\">here</a>");
                }
                catch (CouldNotSendEmailException)
                {
                    return BadRequest("Could not send email");
                }

                return Ok();
            }
            AddErrors(result);

            // If we got this far, something failed
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("verifyEmail")]
        public async Task<IHttpActionResult> ConfirmEmail(ConfirmEmailModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized();

            var result = await UserManager.ConfirmEmailAsync(user.Id, model.Code);
            if (!result.Succeeded)
                return Unauthorized();

            return Ok();
        }

        [HttpPost]
        [Route("forgotPassword")]
        public async Task<IHttpActionResult> ForgotPassword(ForgotRegiPassword model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await UserManager.FindByNameAsync(model.Email);
            if ((user == null) || !await UserManager.IsEmailConfirmedAsync(user.Id))
                return Ok();

            // Send reset code to their email. Needs to be copy and pasted into client
            var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            try
            {
                var uri = GetUri("resetpassword", code);
                await
                    UserManager.SendEmailAsync(user.Id, "Reset Password",
                        $"Password reset authorisation code: {code}<br /><br />Alternatively, click <a href=\"{uri}\">here</a>");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return InternalServerError();
            }

            return Ok();
        }

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
            return BadRequest(ModelState);
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

        private static string GetUri(string action, params string[] parameters)
        {
            var uri = $"evoto://{action}";
            if (parameters.Any())
                uri += "/" + string.Join("/", parameters);
            return uri;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }

        #endregion
    }
}