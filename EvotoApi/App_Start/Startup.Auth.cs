using System;
using EvotoApi.Auth;
using EvotoApi.Models;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace EvotoApi
{
    public partial class Startup
    {

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext<ManaUserManager>(ManaUserManager.Create);
            app.CreatePerOwinContext<ManaSignInManager>(ManaSignInManager.Create);
            

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/manage/login")
            });

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ManaOAuthProvider("ManageClient"),
                AuthorizeEndpointPath = new PathString("/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(1),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true
            });
        }
    }
}