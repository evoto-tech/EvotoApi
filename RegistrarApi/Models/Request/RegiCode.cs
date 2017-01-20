using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace Registrar.Api.Models.Request
{
    [DataContract]
    public class RegiCode
    {
        [DataMember(Name = "selectedProvider")]
        public string SelectedProvider { get; private set; }

        [DataMember(Name = "providers")]
        public ICollection<SelectListItem> Providers { get; private set; }
    }
}