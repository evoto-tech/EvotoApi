using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace EvotoApi.Areas.RegistrarApi.Models.Request
{
    [DataContract]
    public class WebLoginRegiUser
    {
        [DataMember(Name = "email")]
        [Required]
        public string Email { get; }

        [DataMember(Name = "password")]
        [Required]
        public string Password { get; }
    }
}