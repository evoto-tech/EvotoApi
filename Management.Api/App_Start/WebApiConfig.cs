using System.Web.Http;
using System.Web.Mvc;

namespace EvotoApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "Management_default",
                "management/{controller}/{action}",
                new {action = UrlParameter.Optional}
            );

            config.Routes.MapHttpRoute(
                "Management_react_login",
                "manage/login",
                new {controller = "React", action = "Login"}
            );

            config.Routes.MapHttpRoute(
                "Management_react",
                "manage/{*uri}",
                new {controller = "React", action = "Index"}
            );
        }
    }
}