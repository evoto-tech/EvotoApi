﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Management.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using Registrar.Models.Request;

namespace EvotoApi.Areas.ManagementApi.Models.Request
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

        [DataMember(Name = "encryptResults")]
        public bool EncryptResults { get; private set; }

        [DataMember(Name = "blockSpeed")]
        public int BlockSpeed { get; private set; }
    }
}