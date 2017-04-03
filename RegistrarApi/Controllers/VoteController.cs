using System.Threading.Tasks;
using System.Web.Http;
using Blockchain;
using Blockchain.Models;
using Common.Exceptions;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Registrar.Api.Models.Request;
using Registrar.Database.Interfaces;

namespace Registrar.Api.Controllers
{
    [RoutePrefix("vote")]
    public class VoteController : ApiController
    {
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

            // TODO: Check user hasn't had a key signed before

            var keys = RsaTools.LoadKeysFromFile(model.Blockchain);

            var message = new BigInteger(model.BlindedToken);
            var signed = RsaTools.SignBlindedMessage(message, keys.Private);

            return Ok(new {Signature = signed.ToString()});
        }

        [Route("hasvoted")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult HasVoted(HasVotedModel model)
        {
            // TODO: Check if the current user has voted before

            return Ok(new {Voted = true});
        }

        [Route("confirm")]
        [HttpPost]
        public async Task<IHttpActionResult> IssueCurrency(IssueCurrencyModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var blockchain = await _blockchainStore.GetBlockchain(model.Blockchain);

                var keys = RsaTools.LoadKeysFromFile(blockchain.ChainString);

                var signature = new BigInteger(model.BlindSignature);
                if (!RsaTools.VerifySignature(model.Token, signature, keys.Private))
                    return Unauthorized();

                MultichainModel chain;
                if (!_multichaind.Connections.TryGetValue(model.Blockchain, out chain))
                    return NotFound();

                var txId = await chain.IssueVote(model.WalletId);

                return Ok(new { TxId = txId, RegistrarAddress = blockchain.WalletId });
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }

        [Route("key/{blockchain}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPublicKey(string blockchain)
        {
            // Check blockchain exists
            try
            {
                await _blockchainStore.GetBlockchain(blockchain);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }

            var keys = RsaTools.LoadKeysFromFile(blockchain);
            var pub = (RsaKeyParameters) keys.Public;
            return Ok(new
            {
                Modulus = pub.Modulus.ToString(),
                Exponent = pub.Exponent.ToString()
            });
        }
    }
}