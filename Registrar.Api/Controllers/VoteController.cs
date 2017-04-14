using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Blockchain;
using Blockchain.Models;
using Common.Exceptions;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
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
        public async Task<IHttpActionResult> GetBlindSignature(GetBlindSignatureModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get active blockchain connection
            MultichainModel chain;
            if (!_multichaind.Connections.TryGetValue(model.Blockchain, out chain))
                return NotFound();

            // Check user hasn't had a key signed before
            var voters = await chain.GetVoters();
            if (voters.Any(v => v.Id == User.Identity.GetUserId<int>()))
                return Unauthorized();

            // Load key
            var keys = RsaTools.LoadKeysFromFile(model.Blockchain);

            // Blindly sign the token
            var message = new BigInteger(model.BlindedToken);
            var signed = RsaTools.SignBlindedMessage(message, keys.Private);

            // Store the user on the blockchain so they can't have another key
            var voter = new BlockchainVoterModel {Id = User.Identity.GetUserId<int>()};
            await chain.WriteToStream(MultiChainTools.ROOT_STREAM_NAME, MultiChainTools.VOTERS_KEY, voter);

            return Ok(new {Signature = signed.ToString()});
        }

        [Route("hasvoted/{chainString}")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> HasVoted(string chainString)
        {
            // Get active blockchain connection
            MultichainModel chain;
            if (!_multichaind.Connections.TryGetValue(chainString, out chain))
                return NotFound();

            // Check user hasn't had a key signed before
            var voters = await chain.GetVoters();
            var voted = voters.Any(v => v.Id == User.Identity.GetUserId<int>());

            return Ok(new {Voted = voted});
        }

        [Route("confirm")]
        [HttpPost]
        public async Task<IHttpActionResult> IssueCurrency(IssueCurrencyModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var blockchain = await _blockchainStore.GetBlockchainByChainString(model.Blockchain);

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

        [Route("key/{chainString}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPublicKey(string chainString)
        {
            // Check blockchain exists
            try
            {
                await _blockchainStore.GetBlockchainByChainString(chainString);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }

            var keys = RsaTools.LoadKeysFromFile(chainString);
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
                var blockchain = await _blockchainStore.GetBlockchainByChainString(chainString);
                if (blockchain.ExpiryDate > DateTime.UtcNow)
                    return Unauthorized();
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