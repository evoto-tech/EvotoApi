using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Registrar.Models.Request;

namespace Management.Models.Request
{
    [DataContract]
    public class PublishManaVote
    {
        public PublishManaVote(ManaVote vote)
        {
            CreatedBy = vote.CreatedBy;
            Name = vote.Name;
            ExpiryDate = vote.ExpiryDate;
            Published = vote.Published;
            ChainString = vote.ChainString;
            Questions = JsonConvert.DeserializeObject<List<CreateBlockchainQuestion>>(vote.Questions);
            EncryptResults = vote.EncryptResults;
            BlockSpeed = vote.BlockSpeed;
            Published = vote.Published;
            PublishedDate = vote.PublishedDate;
            Info = vote.Info;
        }

        [DataMember(Name = "createdBy")]
        [Required]
        public int CreatedBy { get; private set; }

        [DataMember(Name = "name")]
        [MinLength(2)]
        [MaxLength(100)]
        [Required]
        public string Name { get; private set; }

        [DataMember(Name = "published")]
        [Required]
        public bool Published { get; private set; }

        [DataMember(Name = "expiryDate")]
        [Required]
        public DateTime ExpiryDate { get; private set; }

        [DataMember(Name = "chainString")]
        public string ChainString { get; private set; }

        [DataMember(Name = "questions")]
        public List<CreateBlockchainQuestion> Questions { get; private set; }

        [DataMember(Name = "encrypted")]
        public bool EncryptResults { get; private set; }

        [DataMember(Name = "blockSpeed")]
        public int BlockSpeed { get; private set; }

        [DataMember(Name = "publishedDate")]
        public DateTime? PublishedDate { get; private set; }

        [DataMember(Name = "info")]
        public string Info { get; private set; }
    }
}