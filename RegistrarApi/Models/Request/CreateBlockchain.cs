using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Api.Models.Request
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
    }
}