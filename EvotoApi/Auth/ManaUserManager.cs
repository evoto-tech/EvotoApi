using System;
using System.Web.Mvc;
using Management.Database.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace EvotoApi.Auth
{
    public class ManaUserManager : UserManager<ManaAuthUser, int>
    {
        public ManaUserManager(IUserStore<ManaAuthUser, int> store)
            : base(store)
        {
        }

        public static ManaUserManager Create(IdentityFactoryOptions<ManaUserManager> options,
            IOwinContext context)
        {
            var userStore = (IManaUserStore) DependencyResolver.Current.GetService(typeof(IManaUserStore));
            var lockoutStore =
                (IManaUserLockoutStore) DependencyResolver.Current.GetService(typeof(IManaUserLockoutStore));
            var store = new ManaAuthUserStore(userStore, lockoutStore);
            var manager = new ManaUserManager(store);
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ManaAuthUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 5,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ManaAuthUser, int>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            manager.EmailService = new EmailService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ManaAuthUser, int>(dataProtectionProvider.Create("ASP.NET Evoto Management Identity"));
            return manager;
        }
    }
}