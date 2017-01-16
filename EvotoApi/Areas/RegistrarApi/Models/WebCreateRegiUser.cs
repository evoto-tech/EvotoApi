using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Common;
using Registrar.Models;

namespace EvotoApi.Areas.RegistrarApi.Models
{
    [Serializable]
    public class WebCreateRegiUser
    {
        [DataMember(Name = "firstName")]
        [MinLength(2)]
        [MaxLength(100)]
        [Required]
        public string FirstName { get; }

        [DataMember(Name = "lastName")]
        [MinLength(2)]
        [MaxLength(100)]
        [Required]
        public string LastName { get; }

        [DataMember(Name = "email")]
        [EmailAddress]
        [Required]
        public string Email { get; }

        [DataMember(Name = "password")]
        [DataType(DataType.Password)]
        [Required]

        public string Password { get; }

        [DataMember(Name = "confirmPassword")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ComparePassword { get; }

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