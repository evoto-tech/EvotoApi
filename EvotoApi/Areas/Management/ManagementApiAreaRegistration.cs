using System.Web.Mvc;

namespace EvotoApi.Areas.ManagementApi
{
    public class ManagementApiAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ManagementApi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ManagementApi_Default",
                "mana/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            // TODO: Figure out why this needs to be defined
            context.MapRoute(
                "ManagementApi_Registrar_Relay",
                "regi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}