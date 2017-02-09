using System;
using Registrar.Models;

namespace Registrar.Database.Models
{
    public class RegiDbBlockchain
    {
        public RegiDbBlockchain(dynamic record)
        {
            Name = record.Name;
            ExpiryTime = record.ExpiryTime;
            ChainString = record.ChainString;
        }

        public RegiDbBlockchain(RegiBlockchain model)
        {
            if (model == null)
                throw new ArgumentException(nameof(model));

            Name = model.Name;
            ExpiryTime = model.ExpiryTime;
            ChainString = model.ChainString;
        }

        public string Name { get; }
        public DateTime ExpiryTime { get; }
        public string ChainString { get; }

        public RegiBlockchain ToBlockchain()
        {
            return new RegiBlockchain
            {
                Name = Name,
                ExpiryTime = ExpiryTime,
                ChainString = ChainString
            };
        }
    }
}