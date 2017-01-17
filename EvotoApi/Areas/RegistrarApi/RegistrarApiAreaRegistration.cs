using System.Web.Mvc;

namespace EvotoApi.Areas.RegistrarApi
{
    public class RegistrarApiAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RegistrarApi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RegistrarApi_Default",
                "regi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}