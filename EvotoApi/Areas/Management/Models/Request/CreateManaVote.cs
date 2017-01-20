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
        public int OrgId { get; private set; }

        [DataMember(Name = "createdBy")]
        [Required]
        public int CreatedBy { get; private set; }

        [DataMember(Name = "name")]
        [MinLength(2)]
        [MaxLength(100)]
        [Required]
        public string Name { get; private set; }

        [DataMember(Name = "state")]
        [Required]
        public string State { get; private set; }

        [DataMember(Name = "chainString")]
        [Required]
        public string ChainString { get; private set; }

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