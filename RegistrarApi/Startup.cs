using Microsoft.Owin;
using Owin;
using Registrar.Api;

[assembly: OwinStartup(typeof(Startup))]

namespace Registrar.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}