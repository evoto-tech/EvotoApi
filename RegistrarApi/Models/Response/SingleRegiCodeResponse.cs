using System.Collections.Generic;
using System.Web.Mvc;

namespace Registrar.Api.Models.Response
{
    public class SingleRegiCodeResponse
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}