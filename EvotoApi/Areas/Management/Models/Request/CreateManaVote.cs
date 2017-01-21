using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Common;
using Management.Models;

namespace EvotoApi.Areas.ManagementApi.Models.Request
{
    [DataContract]
    public class CreateManaVote
    {
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

        [DataMember(Name = "expiryDate")]
        [Required]
        public DateTime ExpiryDate { get; private set; }

        [DataMember(Name = "chainString")]
        public string ChainString { get; private set; }

        public ManaVote ToModel()
        {
            return new ManaVote()
            {
                CreatedBy = CreatedBy,
                Name = Name,
                ExpiryDate = ExpiryDate,
                State = State,
                ChainString = ChainString
            };
        }
    }
}