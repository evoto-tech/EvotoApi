using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Api.Models.Request
{
    [DataContract]
    public class VerifyCodeViewModel
    {
        [Required]
        [DataMember(Name = "provider")]
        public string Provider { get; private set; }

        [Required]
        [DataMember(Name = "code")]
        public string Code { get; private set; }

        [DataMember(Name = "returnUrl")]
        public string ReturnUrl { get; private set; }

        [DataMember(Name = "rememberMe")]
        public bool RememberMe { get; private set; }
    }
}