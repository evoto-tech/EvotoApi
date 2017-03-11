using System.Security.Claims;
using System.Threading.Tasks;
using Management.Models;
using Microsoft.AspNet.Identity;

namespace EvotoApi.Auth
{
    public class ManaAuthUser : ManaUser
    {
        public ManaAuthUser()
        {
        }

        public ManaAuthUser(ManaUser user)
        {
            Id = user.Id;
            Email = user.Email;
            PasswordHash = user.PasswordHash;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ManaAuthUser, int> manager,
            string authenticationType = DefaultAuthenticationTypes.ApplicationCookie)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}