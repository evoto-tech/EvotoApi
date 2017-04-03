using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Models.Request
{
    [DataContract]
    public class VerifyRegiUser
    {
        [DataMember(Name = "provider")]
        [Required]
        public string Provider { get; private set; }

        [DataMember(Name = "code")]
        [Required]
        public string Code { get; private set; }
    }
}