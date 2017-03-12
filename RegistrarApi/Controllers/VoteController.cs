using System.Threading.Tasks;
using System.Web.Http;
using Blockchain;
using Blockchain.Models;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Registrar.Api.Models.Request;
using Registrar.Database.Interfaces;

namespace Registrar.Api.Controllers
{
    [RoutePrefix("vote")]
    public class VoteController : ApiController
    {
        // TODO: Make this configurable?
        private const string KEYS_FILE = "evoto.pem";
        private readonly IRegiBlockchainStore _blockchainStore;
        private readonly MultiChainHandler _multichaind;

        public VoteController(IRegiBlockchainStore blockchainStore, MultiChainHandler multichain)
        {
            _blockchainStore = blockchainStore;
            _multichaind = multichain;
        }

        [Route("")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetBlindSignature(GetBlindSignatureModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            MultichainModel chain;
            if (!_multichaind.Connections.TryGetValue(model.Blockchain, out chain))
                return NotFound();

            var keys = RsaTools.LoadKeysFromFile(KEYS_FILE);

            var message = new BigInteger(model.BlindedToken);
            var signed = RsaTools.SignBlindedMessage(message, keys.Private);

            return Ok(new {Signature = signed.ToString()});
        }

        [Route("confirm")]
        [HttpPost]
        public async Task<IHttpActionResult> IssueCurrency(IssueCurrencyModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var blockchain = await _blockchainStore.GetBlockchain(model.Blockchain);

            var keys = RsaTools.LoadKeysFromFile(KEYS_FILE);

            var signature = new BigInteger(model.BlindSignature);
            if (!RsaTools.VerifySignature(model.Token, signature, keys.Private))
                return Unauthorized();

            MultichainModel chain;
            if (!_multichaind.Connections.TryGetValue(model.Blockchain, out chain))
                return NotFound();

            var txId = await chain.IssueVote(model.WalletId);

            return Ok(new {TxId = txId, RegistrarAddress = blockchain.WalletId});
        }

        [Route("key")]
        [HttpGet]
        public IHttpActionResult GetPublicKey()
        {
            var keys = RsaTools.LoadKeysFromFile(KEYS_FILE);
            var pub = (RsaKeyParameters) keys.Public;
            return Ok(new
            {
                Modulus = pub.Modulus.ToString(),
                Exponent = pub.Exponent.ToString()
            });
        }
    }
}