using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Common
{
    public class ApiKeyAuth : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext context)
        {
            var queryString = HttpUtility.ParseQueryString(
                context.Request.RequestUri.Query);

            var key = queryString.Get("key");
            return (key != null) && IsValidKey(key);
        }

        private static bool IsValidKey(string key)
        {
            var keys = ConfigurationManager.AppSettings["ApiKeys"];

            if (string.IsNullOrWhiteSpace(keys))
                return false;

            var keyList = keys.Split(',');
            return keyList.Contains(key);
        }
    }
}