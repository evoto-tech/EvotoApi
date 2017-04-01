using System;

namespace Management.Models
{
    public class ManaVote
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Published { get; set; }
        public string ChainString { get; set; }
        public string Questions { get; set; }
        public bool EncryptResults { get; set; }
        public int BlockSpeed { get; set; }
    }
}