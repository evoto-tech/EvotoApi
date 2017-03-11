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
            Port = record.Port;
            WalletId = record.WalletId;
        }

        public RegiDbBlockchain(RegiBlockchain model)
        {
            if (model == null)
                throw new ArgumentException(nameof(model));

            Name = model.Name;
            ExpiryDate = model.ExpiryDate;
            ChainString = model.ChainString;
            Port = model.Port;
            WalletId = model.WalletId;
        }

        public string Name { get; }
        public DateTime ExpiryDate { get; }
        public string ChainString { get; }
        public int Port { get; }
        public string WalletId { get; }

        public RegiBlockchain ToBlockchain()
        {
            return new RegiBlockchain
            {
                Name = Name,
                ExpiryDate = ExpiryDate,
                ChainString = ChainString,
                Port = Port,
                WalletId = WalletId
            };
        }
    }
}