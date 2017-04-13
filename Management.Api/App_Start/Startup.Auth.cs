using System;
using EvotoApi.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace EvotoApi
{
    public partial class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        public static TimeSpan CookieDuration => TimeSpan.FromMinutes(30);

        public static TimeSpan AuthLockout => TimeSpan.FromMinutes(5);

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext<ManaUserManager>(ManaUserManager.Create);
            app.CreatePerOwinContext<ManaSignInManager>(ManaSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/manage/login"),
                SlidingExpiration = true,
                CookieHttpOnly = true,
                AuthenticationMode = AuthenticationMode.Active,
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ManaUserManager, ManaAuthUser, int>(
                        CookieDuration,
                        (manager, user) => user.GenerateUserIdentityAsync(manager),
                        user => user.GetUserId<int>())
                }
            });
        }
    }
}