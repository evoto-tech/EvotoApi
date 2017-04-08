using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Models.Request
{
    [DataContract]
    public class ChangePasswordModel
    {
        [DataMember(Name = "userId")]
        [Required]
        public int UserId { get; private set; }

        [DataMember(Name = "password")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; private set; }
    }
}