using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace Registrar.Api.Models.Request
{
    [DataContract]
    public class RegiCode
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}