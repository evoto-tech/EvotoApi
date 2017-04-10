using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Models.Request
{
    [DataContract]
    public class CreateRegiUser
    {
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
        [Compare("Password", ErrorMessage = "Entered Passwords do not match")]
        [Required]
        public string ComparePassword { get; private set; }

        [DataMember(Name = "customFields")]
        [Required]
        public IList<CreateRegiUserCustomField> CustomFields { get; private set; }
    }

    [DataContract]
    public class CreateRegiUserCustomField
    {
        [DataMember(Name = "name")]
        [Required]
        public string Name { get; private set; }

        [DataMember(Name = "value")]
        public string Value { get; private set; }
    }
}