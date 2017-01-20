using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Api.Models.Request
{
    [DataContract]
    public class LoginRegiUser
    {
        [DataMember(Name = "email")]
        [Required]
        [EmailAddress]
        public string Email { get; }

        [DataMember(Name = "password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; }

    }
}