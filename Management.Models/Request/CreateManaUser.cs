using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Management.Models.Request
{
    [DataContract]
    public class CreateManaUser
    {
        [DataMember(Name = "email")]
        [Required]
        [EmailAddress]
        public string Email { get; private set; }

        [DataMember(Name = "password")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; private set; }

        [DataMember(Name = "confirmPassword")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Entered Passwords do not match")]
        [Required]
        public string ComparePassword { get; private set; }
    }
}