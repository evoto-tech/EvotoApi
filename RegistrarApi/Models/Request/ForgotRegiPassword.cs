using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Api.Models.Request
{
    [DataContract]
    public class ForgotRegiPassword
    {
        [Required]
        [EmailAddress]
        [DataMember(Name = "Email")]
        public string Email { get; private set; }
    }
}