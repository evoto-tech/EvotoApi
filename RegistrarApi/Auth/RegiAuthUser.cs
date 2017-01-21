﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Registrar.Models;

namespace Registrar.Api.Auth
{
    public class RegiAuthUser : RegiUser
    {
        public RegiAuthUser()
        {
        }

        public RegiAuthUser(RegiUser user)
        {
            Id = user.Id;
            Email = user.Email;
            PasswordHash = user.PasswordHash;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<RegiAuthUser, int> manager,
            string authenticationType = DefaultAuthenticationTypes.ExternalBearer)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}