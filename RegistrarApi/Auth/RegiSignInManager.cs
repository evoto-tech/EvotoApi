using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Registrar.Api.Auth
{
    public class RegiSignInManager : SignInManager<RegiAuthUser, string>
    {
        public RegiSignInManager(RegiUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(RegiAuthUser user)
        {
            return user.GenerateUserIdentityAsync((RegiUserManager) UserManager);
        }

        public static RegiSignInManager Create(IdentityFactoryOptions<RegiSignInManager> options,
            IOwinContext context)
        {
            return new RegiSignInManager(context.GetUserManager<RegiUserManager>(), context.Authentication);
        }
    }
}