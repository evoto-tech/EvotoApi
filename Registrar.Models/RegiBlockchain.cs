﻿using System;

namespace Registrar.Models
{
    public class RegiBlockchain
    {
        public string Name { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string ChainString { get; set; }

        public string Info { get; set; }

        public int Port { get; set; }

        public string WalletId { get; set; }
    }
}