using System.Web.Mvc;
using System.Web.Routing;

namespace EvotoApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "Management_react_login",
                "manage/login",
                new {controller = "React", action = "Login"}
            );

            routes.MapRoute(
                "Management_react",
                "manage/{*uri}",
                new {controller = "React", action = "Index"}
            );
        }
    }
}