using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Models.Request
{
    [DataContract]
    public class CreateBlockchain
    {
        [DataMember]
        [Required]
        public string Name { get; private set; }

        [DataMember]
        [Required]
        public DateTime ExpiryDate { get; private set; }

        [DataMember]
        [Required]
        public string ChainString { get; private set; }

        [DataMember]
        public string Info { get; private set; }

        [DataMember]
        [Required]
        public List<CreateBlockchainQuestion> Questions { get; private set; }
    }

    [DataContract]
    public class CreateBlockchainQuestion
    {
        [DataMember]
        [Required]
        public string Question { get; private set; }

        [DataMember]
        public string Info { get; private set; }

        [DataMember]
        [Required]
        public List<CreateBlockchainAnswer> Answers { get; private set; }
    }

    [DataContract]
    public class CreateBlockchainAnswer
    {
        [DataMember]
        [Required]
        public string Answer { get; private set; }

        [DataMember]
        public string Info { get; private set; }
    }
}