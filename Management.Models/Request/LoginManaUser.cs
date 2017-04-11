using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Management.Models.Request
{
    [DataContract]
    public class LoginManaUser
    {
        [DataMember(Name = "email")]
        [Required]
        [EmailAddress]
        public string Email { get; private set; }

        [DataMember(Name = "password")]
        [Required]
        public string Password { get; private set; }
    }
}