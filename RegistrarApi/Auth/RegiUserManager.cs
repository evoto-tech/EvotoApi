using System;
using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Registrar.Database.Interfaces;
using Registrar.Database.Stores;

namespace Registrar.Api.Auth
{
    public class RegiUserManager : UserManager<RegiAuthUser>
    {
        public RegiUserManager(IUserStore<RegiAuthUser> store)
            : base(store)
        {
        }

        public static RegiUserManager Create(IdentityFactoryOptions<RegiUserManager> options,
            IOwinContext context)
        {
            // TODO
            var store = new RegiAuthUserStore(new RegiSqlUserStore(ConfigurationManager.ConnectionStrings["somet"].ConnectionString));
            var manager = new RegiUserManager(store);
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<RegiAuthUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<RegiAuthUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<RegiAuthUser>(dataProtectionProvider.Create("ASP.NET Evoto Registrar Identity"));
            return manager;
        }
    }
}