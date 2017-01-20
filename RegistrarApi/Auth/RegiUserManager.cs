﻿using System;
using System.Configuration;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Registrar.Database.Interfaces;
using Registrar.Database.Stores;

namespace Registrar.Api.Auth
{
    public class RegiUserManager : UserManager<RegiAuthUser, int>
    {
        public RegiUserManager(IUserStore<RegiAuthUser, int> store)
            : base(store)
        {
        }

        public static RegiUserManager Create(IdentityFactoryOptions<RegiUserManager> options,
            IOwinContext context)
        {
            var userStore = (IRegiUserStore) DependencyResolver.Current.GetService(typeof(IRegiUserStore));
            var lockoutStore =
                (IRegiUserLockoutStore) DependencyResolver.Current.GetService(typeof(IRegiUserLockoutStore));
            var store = new RegiAuthUserStore(userStore, lockoutStore);
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

            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<RegiAuthUser, int>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<RegiAuthUser, int>(dataProtectionProvider.Create("ASP.NET Evoto Registrar Identity"));
            return manager;
        }
    }
}