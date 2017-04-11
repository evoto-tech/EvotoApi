using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace EvotoApi.Auth
{
    public class ManaSignInManager : SignInManager<ManaAuthUser, int>
    {
        public ManaSignInManager(ManaUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ManaAuthUser user)
        {
            return user.GenerateUserIdentityAsync((ManaUserManager) UserManager);
        }

        public static ManaSignInManager Create(IdentityFactoryOptions<ManaSignInManager> options,
            IOwinContext context)
        {
            return new ManaSignInManager(context.GetUserManager<ManaUserManager>(), context.Authentication);
        }
    }
}