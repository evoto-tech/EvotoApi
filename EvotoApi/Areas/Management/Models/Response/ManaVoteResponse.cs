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
            OrgId = vote.OrgId;
            CreatedBy = vote.CreatedBy;
            Name = vote.Name;
            CreationDate = vote.CreationDate;
            ExpiryDate = vote.ExpiryDate;
            State = vote.State;
            ChainString = vote.ChainString;
        }

        [DataMember(Name = "id")]
        public int Id { get; }

        [DataMember(Name = "orgId")]
        public int OrgId { get; }

        [DataMember(Name = "createdBy")]
        public int CreatedBy { get; }

        [DataMember(Name = "name")]
        public string Name { get; }

        [DataMember(Name = "creationDate")]
        public DateTime CreationDate { get; }

        [DataMember(Name = "expiryDate")]
        public DateTime ExpiryDate { get; }

        [DataMember(Name = "state")]
        public string State { get; }

        [DataMember(Name = "chainString")]
        public string ChainString { get; }
    }
}