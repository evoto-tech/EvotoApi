using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Management.Models.Request
{
    [DataContract]
    public class CreateManaVote
    {
        [DataMember(Name = "createdBy")]
        [Required]
        public int CreatedBy { get; set; }

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
        [RegularExpression("^[a-zA-Z][a-zA-Z0-9]+$")]
        public string ChainString { get; private set; }

        [DataMember(Name = "questions")]
        public string Questions { get; private set; }

        [DataMember(Name = "encryptResults")]
        public bool EncryptResults { get; private set; }

        [DataMember(Name = "blockSpeed")]
        public int BlockSpeed { get; private set; }

        [DataMember(Name = "publishedDate")]
        public DateTime? PublishedDate { get; set; }

        [DataMember(Name = "info")]
        public string Info { get; set; }

        public ManaVote ToModel()
        {
            return new ManaVote
            {
                CreatedBy = CreatedBy,
                Name = Name,
                ExpiryDate = ExpiryDate,
                Published = Published,
                ChainString = ChainString,
                Questions = Questions,
                EncryptResults = EncryptResults,
                BlockSpeed = BlockSpeed,
                PublishedDate = PublishedDate,
                Info = Info
            };
        }
    }
}