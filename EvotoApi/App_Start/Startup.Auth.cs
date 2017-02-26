using System;
using EvotoApi.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace EvotoApi
{
    public partial class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        // In minutes
        public static int RefreshTokenTime => 30;

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext<ManaUserManager>(ManaUserManager.Create);
            app.CreatePerOwinContext<ManaSignInManager>(ManaSignInManager.Create);

            OAuthBearerOptions =
                new OAuthBearerAuthenticationOptions();
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ExternalBearer,
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ManaOAuthProvider("Management"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(15),
                AllowInsecureHttp = true,
                RefreshTokenProvider = new ManaRefreshTokenProvider()
            });
        }
    }
}