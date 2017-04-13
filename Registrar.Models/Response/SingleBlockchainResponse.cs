using System;
using System.Runtime.Serialization;

namespace Registrar.Models.Response
{
    [DataContract]
    public class SingleBlockchainResponse
    {
        public SingleBlockchainResponse(RegiBlockchain blockchain, int blocks)
        {
            Name = blockchain.Name;
            ExpiryDate = blockchain.ExpiryDate;
            ChainString = blockchain.ChainString;
            Info = blockchain.Info;
            Port = blockchain.Port;
            Host = blockchain.Host;
            WalletId = blockchain.WalletId;
            Blocks = blocks;
            EncryptKey = blockchain.EncryptKey;
        }

        [DataMember(Name = "name")]
        public string Name { get; }

        [DataMember(Name = "expiryDate")]
        public DateTime ExpiryDate { get; }

        [DataMember(Name = "chainString")]
        public string ChainString { get; }

        [DataMember(Name = "info")]
        public string Info { get; }

        [DataMember(Name = "port")]
        public int Port { get; }

        [DataMember(Name = "host")]
        public string Host { get; }

        [DataMember(Name = "walletId")]
        public string WalletId { get; }

        [DataMember(Name = "blocks")]
        public int Blocks { get; }

        [DataMember(Name = "encryptKey")]
        public string EncryptKey { get; }
    }
}