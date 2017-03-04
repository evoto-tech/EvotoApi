using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Api.Models.Request
{
    [DataContract]
    public class IssueCurrencyModel
    {
        [DataMember(Name = "blockchain")]
        [Required]
        public string Blockchain { get; private set; }

        [DataMember(Name = "walletId")]
        [Required]
        public string WalletId { get; private set; }

        [DataMember(Name = "token")]
        [Required]
        public string Token { get; private set; }

        [DataMember(Name = "blindSignature")]
        [Required]
        public string BlindSignature { get; private set; }
    }
}