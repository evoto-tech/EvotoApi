using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Models.Request
{
    [DataContract]
    public class ChangePasswordModel
    {
        [DataMember(Name = "userId")]
        [Required]
        public int UserId { get; set; }

        [DataMember(Name = "password")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}