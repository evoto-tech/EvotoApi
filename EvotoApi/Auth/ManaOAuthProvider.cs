using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

namespace EvotoApi.Auth
{
    public class ManaOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        private ManaSignInManager _signInManager;
        private ManaUserManager _userManager;

        public ManaOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
                throw new ArgumentNullException(nameof(publicClientId));

            _publicClientId = publicClientId;
        }

        public ManaSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ManaSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ManaUserManager UserManager
        {
            get { return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ManaUserManager>(); }
            private set { _userManager = value; }
        }

        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {
            context.AdditionalResponseParameters.Add("userId", context.Identity.GetUserId());
            return base.TokenEndpointResponse(context);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var result = await SignInManager.PasswordSignInAsync(context.UserName, context.Password, false, true);

            switch (result)
            {
                case SignInStatus.Success:

                    var user = await UserManager.FindByNameAsync(context.UserName);

                    var oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                        OAuthDefaults.AuthenticationType);
                    var cookiesIdentity = await user.GenerateUserIdentityAsync(UserManager,
                        CookieAuthenticationDefaults.AuthenticationType);

                    var properties = CreateProperties(user.UserName);
                    var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                    context.Validated(ticket);
                    context.Request.Context.Authentication.SignIn(cookiesIdentity);

                    break;
                case SignInStatus.LockedOut:
                    context.SetError("invalid_grant", "Too many incorrect attempts.");
                    return;
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new {ReturnUrl = returnUrl, model.RememberMe});
                default:
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
                context.AdditionalResponseParameters.Add(property.Key, property.Value);

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
                context.Validated();

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
                context.Validated();

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                {"userName", userName}
            };
            return new AuthenticationProperties(data);
        }
    }
}