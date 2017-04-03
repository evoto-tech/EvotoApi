using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Models.Request
{
    [DataContract]
    public class LoginRegiUser
    {
        [DataMember(Name = "email")]
        [Required]
        [EmailAddress]
        public string Email { get; private set; }

        [DataMember(Name = "password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; private set; }
    }
}