using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Models.Request
{
    [DataContract]
    public class ConfirmEmailModel
    {
        [Required]
        [EmailAddress]
        [DataMember(Name = "email")]
        public string Email { get; private set; }

        [Required]
        [DataMember(Name = "code")]
        public string Code { get; private set; }
    }
}