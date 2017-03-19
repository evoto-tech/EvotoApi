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
            Published = record.Published;
            ChainString = record.ChainString;
            Questions = record.Questions;
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
            Published = model.Published;
            ChainString = model.ChainString;
            Questions = model.Questions;
        }

        public int Id { get; }
        public int CreatedBy { get; }
        public string Name { get; }
        public DateTime CreationDate { get; }
        public DateTime ExpiryDate { get; }
        public bool Published { get; }
        public string ChainString { get; }
        public string Questions { get; }

        public ManaVote ToVote()
        {
            return new ManaVote
            {
                Id = Id,
                CreatedBy = CreatedBy,
                Name = Name,
                CreationDate = CreationDate,
                ExpiryDate = ExpiryDate,
                Published = Published,
                ChainString = ChainString,
                Questions = Questions
            };
        }
    }
}