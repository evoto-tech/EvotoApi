using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Api.Models.Request
{
    [DataContract]
    public class GetBlindSignatureModel
    {
        [DataMember(Name = "blockchain")]
        [Required]
        public string Blockchain { get; private set; }

        [DataMember(Name = "token")]
        [Required]
        public string BlindedToken { get; private set; }
    }
}