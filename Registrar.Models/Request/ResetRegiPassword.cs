﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Models.Request
{
    [DataContract]
    public class ResetRegiPassword
    {
        [DataMember(Name = "email")]
        [Required]
        [EmailAddress]
        public string Email { get; private set; }

        [DataMember(Name = "password")]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; private set; }

        [DataMember(Name = "confirmPassword")]
        [DataType(DataType.Password)]
        [Compare("Password",
             ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; private set; }

        [DataMember(Name = "code")]
        public string Code { get; private set; }
    }
}