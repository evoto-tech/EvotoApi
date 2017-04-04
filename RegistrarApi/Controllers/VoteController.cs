using System;
using System.Threading.Tasks;
using System.Web.Http;
using Blockchain;
using Blockchain.Models;
using Common.Exceptions;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Registrar.Database.Interfaces;
using Registrar.Models;
using Registrar.Models.Request;

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

                string words;
                do
                {
                    words = string.Join(" ", RandomWordsGenerator.GetRandomWords());
                } while (await chain.CheckMagicWordsNotOnBlockchain(words, model.WalletId)); 

                return Ok(new {TxId = txId, RegistrarAddress = blockchain.WalletId, Words = words});
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
            return Ok(new
            {
                PublicKey = RsaTools.KeyToString(keys.Public)
            });
        }

        [Route("decrypt/{chainString}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDecryptKey(string chainString)
        {
            // Check blockchain exists
            try
            {
                var blockchain = await _blockchainStore.GetBlockchain(chainString);
                if (blockchain.ExpiryDate > DateTime.Now)
                {
                    return Unauthorized();
                }
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }

            var keys = RsaTools.LoadKeysFromFile(chainString + "-encrypt");
            return Ok(new
            {
                PrivateKey = RsaTools.KeyToString(keys.Private)
            });
        }
    }
}