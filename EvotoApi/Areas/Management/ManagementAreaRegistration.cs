using System.Web.Mvc;

namespace EvotoApi.Areas.Management
{
    public class ManagementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Management";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Management_default",
                "management/{controller}/{action}",
                new { action = UrlParameter.Optional }
            );

            context.MapRoute(
                "Management_react_login",
                "manage/login",
                new { controller = "React", action = "Login" }
            );

            context.MapRoute(
                "Management_react",
                "manage/{*uri}",
                new {controller = "React", action = "Index"}
            );
        }
    }
}