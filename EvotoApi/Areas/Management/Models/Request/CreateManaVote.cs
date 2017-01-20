using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Common;
using Management.Models;

namespace EvotoApi.Areas.ManagementApi.Models.Request
{
    [DataContract]
    public class CreateManaVote
    {
        [DataMember(Name = "orgId")]
        [Required]
        public int OrgId { get; }

        [DataMember(Name = "createdBy")]
        [Required]
        public int CreatedBy { get; }

        [DataMember(Name = "name")]
        [MinLength(2)]
        [MaxLength(100)]
        [Required]
        public string Name { get; }

        [DataMember(Name = "state")]
        [Required]
        public string State { get; }

        [DataMember(Name = "chainString")]
        [Required]
        public string ChainString { get; }

        public string Password { get; }

        public ManaVote ToModel()
        {
            return new ManaVote()
            {
                OrgId = OrgId,
                CreatedBy = CreatedBy,
                Name = Name,
                State = State,
                ChainString = ChainString
            };
        }
    }
}