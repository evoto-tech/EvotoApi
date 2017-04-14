using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Registrar.Database.Interfaces;
using Registrar.Models;

namespace Registrar.Api.Auth
{
    public class RegiUserManager : UserManager<RegiAuthUser, int>
    {
        public RegiUserManager(IUserStore<RegiAuthUser, int> store)
            : base(store)
        {
        }

        private static T Get<T>()
        {
            return (T) DependencyResolver.Current.GetService(typeof(T));
        }

        public static RegiUserManager Create(IdentityFactoryOptions<RegiUserManager> options,
            IOwinContext context)
        {
            var userStore = Get<IRegiUserStore>();
            var lockoutStore = Get<IRegiUserLockoutStore>();
            var fieldStore = Get<IRegiUserFieldsStore>();
            var tokenStore = Get<IRegiRefreshTokenStore>();

            var store = new RegiAuthUserStore(userStore, lockoutStore, fieldStore, tokenStore);
            var manager = new RegiUserManager(store);
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<RegiAuthUser, int>(manager)
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

            //manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<RegiAuthUser, int>
            //{
            //    Subject = "Security Code",
            //    BodyFormat = "Your security code is {0}"
            //});

            manager.UserTokenProvider = new RegiTokenProvider();

            manager.EmailService = new EmailService();
            return manager;
        }

        public async Task<CanSendEmailModel> CanSendToken(string purpose, RegiAuthUser user)
        {
            var tokenProvider = UserTokenProvider as RegiTokenProvider;
            if (tokenProvider == null)
                return new CanSendEmailModel(false, TimeSpan.Zero);

            // Get any existing token
            var token = await tokenProvider.GetToken(purpose, user);
            // This should only happen if the token table has been flushed
            if (token == null)
                return new CanSendEmailModel(true, TimeSpan.Zero);

            // Time since the email was sent
            var delay = DateTime.UtcNow - token.Created;
            // Ensure the minimum time has passed
            var canSend = delay > Startup.EmailDelay;

            // Time left to wait
            var remaining = Startup.EmailDelay - delay;
            return new CanSendEmailModel(canSend, remaining);
        }
    }
}