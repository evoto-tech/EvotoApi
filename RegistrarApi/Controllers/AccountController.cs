using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Common.Exceptions;
using Common.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Registrar.Api.Auth;
using Registrar.Api.Models;
using Registrar.Api.Models.Request;
using Registrar.Api.Models.Response;
using Registrar.Database.Interfaces;

namespace Registrar.Api.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : ApiController
    {
        private readonly IRegiUserFieldsStore _fieldsStore;
        private RegiSignInManager _signInManager;
        private RegiUserManager _userManager;

        public AccountController(IRegiUserFieldsStore fieldsStore)
        {
            _fieldsStore = fieldsStore;
        }

        public RegiSignInManager SignInManager
            => _signInManager ?? (_signInManager = HttpContext.Current.GetOwinContext().Get<RegiSignInManager>());

        public RegiUserManager UserManager
            => _userManager ?? (_userManager = HttpContext.Current.GetOwinContext().Get<RegiUserManager>());

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

            // Validate custom fields
            var fields = await _fieldsStore.GetCustomUserFields();
            var errors = ValidateCustomUserFields(model, fields);

            if (errors.Any())
            {
                AddErrors(errors);
                return BadRequest(ModelState);
            }

            var user = new RegiAuthUser {UserName = model.Email, Email = model.Email};
            var result = await UserManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                AddErrors(result);
                return BadRequest(ModelState);
            }

            // Get created account
            var userModel = await UserManager.FindByEmailAsync(model.Email);

            // Store custom user data
            var fieldsTasks = fields.Where(f => model.CustomFields.Any(m => m.Name == f.Name))
                .Select(field => new CustomUserValue
                {
                    FieldId = field.Id,
                    Value = model.CustomFields.Single(f => f.Name == field.Name).Value
                }).Select(value => _fieldsStore.AddFieldValueForUser(userModel, value));
            await Task.WhenAll(fieldsTasks);

            // Send an email with this link
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(userModel.Id);
            var body = EmailContentWriter.ConfirmEmail(user.Email, code);
            try
            {
                await UserManager.SendEmailAsync(userModel.Id, EmailContentWriter.ConfirmEmailSubject, body);
            }
            catch (CouldNotSendEmailException)
            {
                return BadRequest("Could not send email");
            }

            return Ok();
        }

        [HttpPost]
        [Route("resendEmail")]
        public async Task<IHttpActionResult> ResendEmail(ResendVerificationEmail model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get account
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized();

            // Ensure the user hasn't had a token sent recently
            var canSendModel = await UserManager.CanSendToken(RegiTokenProvider.EmailProvider, user);
            if (!canSendModel.CanSend)
                return BadRequest($"Please wait {Math.Ceiling(canSendModel.Remaining.TotalMinutes)} minutes");

            // Generate a new token
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

            // Send an email with this link
            var body = EmailContentWriter.ConfirmEmail(user.Email, code);
            try
            {
                await UserManager.SendEmailAsync(user.Id, EmailContentWriter.ConfirmEmailSubject, body);
            }
            catch (CouldNotSendEmailException)
            {
                return InternalServerError();
            }

            return Ok();
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
            // User not found, but we don't want to reveal this
            if (user == null)
                return Ok();

            // Email not confirmed, so can't use for reset
            if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                return BadRequest("Unconfirmed Email");

            // Ensure the user hasn't had a token sent recently
            var canSendModel = await UserManager.CanSendToken(RegiTokenProvider.PasswordProvider, user);
            if (!canSendModel.CanSend)
                return BadRequest($"Please wait {Math.Ceiling(canSendModel.Remaining.TotalMinutes)} minutes");

            // Send reset code to their email. Needs to be copy and pasted into client
            var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var body = EmailContentWriter.ResetPassword(user.Email, code);
            try
            {
                await UserManager.SendEmailAsync(user.Id, EmailContentWriter.ResetPasswordSubject, body);
            }
            catch (Exception)
            {
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

            // Check the password is valid before consuming the token
            var res = await UserManager.PasswordValidator.ValidateAsync(model.Password);
            if (!res.Succeeded)
            {
                AddErrors(res);
                return BadRequest(ModelState);
            }

            // Change password and attempt to consume token
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
                return Ok();

            // Error changing the password. Probably token related
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

        private static List<string> ValidateCustomUserFields(CreateRegiUser model, IEnumerable<CustomUserField> fields)
        {
            var errors = new List<string>();
            foreach (var field in fields)
            {
                CreateRegiUserCustomField userField;
                try
                {
                    userField = model.CustomFields.SingleOrDefault(f => f.Name == field.Name);
                }
                catch (InvalidOperationException)
                {
                    errors.Add($"More than one value for field: {field.Name}");
                    continue;
                }

                if (userField != null)
                {
                    var missing = string.IsNullOrEmpty(userField.Value);
                    if (missing && field.Required)
                        errors.Add($"Missing value for required field: {field.Name}");
                    else
                    {
                        List<string> fieldErrors;
                        if (!missing && !field.IsValid(userField.Value, out fieldErrors))
                            errors.Add($"Invalid value for field: {field.Name}.\n" + string.Join("\n", fieldErrors));
                    }
                    // Else present and valid
                }
                else if (field.Required)
                {
                    errors.Add($"Missing value for required field: {field.Name}");
                }
                // Else missing but not required
            }
            return errors;
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            AddErrors(result.Errors);
        }

        private void AddErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
                ModelState.AddModelError("", error);
        }

        #endregion
    }
}