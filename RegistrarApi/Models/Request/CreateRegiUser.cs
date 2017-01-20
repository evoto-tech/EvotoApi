using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Common;
using Registrar.Models;

namespace Registrar.Api.Models.Request
{
    [DataContract]
    public class CreateRegiUser
    {
        [DataMember(Name = "firstName")]
        [MinLength(2)]
        [MaxLength(100)]
        [Required]
        public string FirstName { get; private set; }

        [DataMember(Name = "lastName")]
        [MinLength(2)]
        [MaxLength(100)]
        [Required]
        public string LastName { get; private set; }

        [DataMember(Name = "email")]
        [EmailAddress]
        [Required]
        public string Email { get; private set; }

        [DataMember(Name = "password")]
        [DataType(DataType.Password)]
        [Required]

        public string Password { get; private set; }

        [DataMember(Name = "confirmPassword")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ComparePassword { get; private set; }

        public RegiUser ToModel()
        {
            return new RegiUser
            {
                Email = Email,
                PasswordHash = Passwords.HashPassword(Password)
            };
        }
    }
}