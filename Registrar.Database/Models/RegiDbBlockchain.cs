using System;
using Registrar.Models;

namespace Registrar.Database.Models
{
    public class RegiDbBlockchain
    {
        public RegiDbBlockchain(dynamic record)
        {
            Name = record.Name;
            ExpiryDate = record.ExpiryDate;
            ChainString = record.ChainString;
        }

        public RegiDbBlockchain(RegiBlockchain model)
        {
            if (model == null)
                throw new ArgumentException(nameof(model));

            Name = model.Name;
            ExpiryDate = model.ExpiryDate;
            ChainString = model.ChainString;
        }

        public string Name { get; }
        public DateTime ExpiryDate { get; }
        public string ChainString { get; }

        public RegiBlockchain ToBlockchain()
        {
            return new RegiBlockchain
            {
                Name = Name,
                ExpiryDate = ExpiryDate,
                ChainString = ChainString
            };
        }
    }
}