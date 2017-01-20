using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Registrar.Models;

namespace Registrar.Api.Auth
{
    public class RegiAuthUser : RegiUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<RegiAuthUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public RegiAuthUser()
        {
            
        }

        public RegiAuthUser(RegiUser user)
        {
            Id = user.Id;
            Email = user.Email;
            PasswordHash = user.PasswordHash;
        }
    }
}