using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Api.Models.Request
{
    [DataContract]
    public class VerifyCodeViewModel
    {
        [Required]
        [DataMember(Name = "provider")]
        public string Provider { get; set; }

        [Required]
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "returnUrl")]
        public string ReturnUrl { get; set; }

        [DataMember(Name = "rememberMe")]
        public bool RememberMe { get; set; }
    }
}