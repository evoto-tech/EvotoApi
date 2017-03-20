using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Registrar.Api.Auth;

namespace Registrar.Api
{
    public partial class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        // Length of validity of Refresh Tokens
        public static TimeSpan RefreshTokenTime => TimeSpan.FromMinutes(30);

        // Timeout for password reset, 2FA and email confirmation tokens
        public static TimeSpan UserTokenTime => TimeSpan.FromHours(24);

        // Minimum time between emails for the same purpose
        public static TimeSpan EmailDelay => TimeSpan.FromMinutes(10);

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext<RegiUserManager>(RegiUserManager.Create);
            app.CreatePerOwinContext<RegiSignInManager>(RegiSignInManager.Create);

            OAuthBearerOptions =
                new OAuthBearerAuthenticationOptions();
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ExternalBearer,
                TokenEndpointPath = new PathString("/Token"),
                Provider = new RegiOAuthProvider("EvotoClient"),
                AuthorizeEndpointPath = new PathString("/external/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(5),
                AllowInsecureHttp = true,
                RefreshTokenProvider = new RegiRefreshTokenProvider()
            });

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            //app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            //app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }
    }
}