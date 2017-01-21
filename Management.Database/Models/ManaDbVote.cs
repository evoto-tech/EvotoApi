using System;
using Management.Models;

namespace Management.Database.Models
{
    public class ManaDbVote
    {
        public ManaDbVote(dynamic record)
        {
            Id = record.Id;
            CreatedBy = record.CreatedBy;
            Name = record.Name;
            CreationDate = record.CreationDate;
            ExpiryDate = record.ExpiryDate;
            State = record.State;
            ChainString = record.ChainString;
        }

        public ManaDbVote(ManaVote model)
        {
            if (model == null)
                throw new ArgumentException(nameof(model));

            Id = model.Id;
            CreatedBy = model.CreatedBy;
            Name = model.Name;
            CreationDate = model.CreationDate;
            ExpiryDate = model.ExpiryDate;
            State = model.State;
            ChainString = model.ChainString;
        }

        public int Id { get; }
        public int CreatedBy { get; }
        public string Name { get; }
        public DateTime CreationDate { get; }
        public DateTime ExpiryDate { get; }
        public string State { get; }
        public string ChainString { get; }

        public ManaVote ToVote()
        {
            return new ManaVote
            {
                Id = Id,
                CreatedBy = CreatedBy,
                Name = Name,
                CreationDate = CreationDate,
                ExpiryDate = ExpiryDate,
                State = State,
                ChainString = ChainString
            };
        }
    }
}