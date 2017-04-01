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
            Info = record.Info;
            Port = record.Port;
            WalletId = record.WalletId;
            EncryptKey = record.EncryptKey;
        }

        public RegiDbBlockchain(RegiBlockchain model)
        {
            if (model == null)
                throw new ArgumentException(nameof(model));

            Name = model.Name;
            ExpiryDate = model.ExpiryDate;
            ChainString = model.ChainString;
            Info = model.Info;
            Port = model.Port;
            WalletId = model.WalletId;
            EncryptKey = model.EncryptKey;
        }

        public string Name { get; }
        public DateTime ExpiryDate { get; }
        public string ChainString { get; }
        public string Info { get; }
        public int Port { get; }
        public string WalletId { get; }
        public string EncryptKey { get; }

        public RegiBlockchain ToBlockchain()
        {
            return new RegiBlockchain
            {
                Name = Name,
                ExpiryDate = ExpiryDate,
                ChainString = ChainString,
                Info = Info,
                Port = Port,
                WalletId = WalletId,
                EncryptKey = EncryptKey
            };
        }
    }
}