using System;
using Microsoft.AspNet.Identity;

namespace Registrar.Models
{
    public class RegiBlockchain
    {
        public string Name { get; set; }

        public DateTime ExpiryTime { get; set; }

        public string ChainString { get; set; }
    }
}