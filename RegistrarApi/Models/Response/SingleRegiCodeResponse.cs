using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace Registrar.Api.Models.Response
{
    [DataContract]
    public class SingleRegiCodeResponse
    {
        [DataMember(Name = "selectedProvider")]
        public string SelectedProvider { get; set; }

        [DataMember(Name = "providers")]
        public ICollection<SelectListItem> Providers { get; set; }
    }
}