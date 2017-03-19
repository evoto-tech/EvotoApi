using System;
using System.Runtime.Serialization;
using Management.Models;

namespace EvotoApi.Areas.ManagementApi.Models.Response
{
    [DataContract]
    public class ManaVoteResponse
    {
        public ManaVoteResponse(ManaVote vote)
        {
            Id = vote.Id;
            CreatedBy = vote.CreatedBy;
            Name = vote.Name;
            CreationDate = vote.CreationDate;
            ExpiryDate = vote.ExpiryDate;
            Published = vote.Published;
            ChainString = vote.ChainString;
            Questions = vote.Questions;
        }

        [DataMember(Name = "id")]
        public int Id { get; }

        [DataMember(Name = "createdBy")]
        public int CreatedBy { get; }

        [DataMember(Name = "name")]
        public string Name { get; }

        [DataMember(Name = "creationDate")]
        public DateTime CreationDate { get; }

        [DataMember(Name = "expiryDate")]
        public DateTime ExpiryDate { get; }

        [DataMember(Name = "published")]
        public bool Published { get; }

        [DataMember(Name = "chainString")]
        public string ChainString { get; }

        [DataMember(Name = "questions")]
        public string Questions { get; }
    }
}