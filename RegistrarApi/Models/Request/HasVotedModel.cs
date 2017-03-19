using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Api.Models.Request
{
    [DataContract]
    public class HasVotedModel
    {
        [DataMember(Name = "blockchain")]
        [Required]
        public string Blockchain { get; private set; }
    }
}