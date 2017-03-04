using System.Threading.Tasks;
using System.Web.Http;
using Blockchain;
using Blockchain.Models;
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

            //var blockchain = await _blockchainStore.GetBlockchain(model.Blockchain);

            MultichainModel chain;
            if (!_multichaind.Connections.TryGetValue(model.Blockchain, out chain))
                return NotFound();

            return Ok(new {Signature = "signature"});
        }

        [Route("confirm")]
        [HttpPost]
        public async Task<IHttpActionResult> IssueCurrency(IssueCurrencyModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.BlindSignature != "signature")
                return Unauthorized();

            var blockchain = await _blockchainStore.GetBlockchain(model.Blockchain);

            MultichainModel chain;
            if (!_multichaind.Connections.TryGetValue(model.Blockchain, out chain))
                return NotFound();

            var txId = await chain.IssueVote(model.WalletId);

            return Ok(new {TxId = txId, RegistrarAddress = blockchain.WalletId});
        }
    }
}