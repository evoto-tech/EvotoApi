using System;
using Common;

namespace Registrar.Models
{
    public class RegiBlockchain
    {
        public string Name { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string ChainString { get; set; }

        public string Info { get; set; }

        public int Port { get; set; }

        public string Host => RegiSettings.BLOCKCHAIN_HOST;

        public string WalletId { get; set; }

        public string EncryptKey { get; set; }
    }
}