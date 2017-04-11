using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Models.Request
{
    [DataContract]
    public class CreateBlockchain
    {
        [DataMember(Name = "name")]
        [Required]
        public string Name { get; private set; }

        [DataMember(Name = "expiryDate")]
        [Required]
        public DateTime ExpiryDate { get; private set; }

        [DataMember(Name = "chainString")]
        [Required]
        public string ChainString { get; private set; }

        [DataMember(Name = "info")]
        public string Info { get; private set; }

        [DataMember(Name = "questions")]
        [Required]
        public List<CreateBlockchainQuestion> Questions { get; private set; }

        [DataMember(Name = "blockSpeed")]
        public int BlockSpeed { get; private set; }

        [DataMember(Name = "encrypted")]
        public bool Encrypted { get; private set; }
    }

    [DataContract]
    public class CreateBlockchainQuestion
    {
        [DataMember(Name = "question")]
        [Required]
        public string Question { get; private set; }

        [DataMember(Name = "info")]
        public string Info { get; private set; }

        [DataMember]
        [Required]
        public List<CreateBlockchainAnswer> Answers { get; private set; }
    }

    [DataContract]
    public class CreateBlockchainAnswer
    {
        [DataMember(Name = "answer")]
        [Required]
        public string Answer { get; private set; }

        [DataMember(Name = "info")]
        public string Info { get; private set; }
    }
}