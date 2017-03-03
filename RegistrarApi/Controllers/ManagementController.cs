using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Blockchain;
using Blockchain.Models;
using Common;
using Registrar.Api.Models.Request;
using Registrar.Database.Interfaces;
using Registrar.Models;

namespace Registrar.Api.Controllers
{
    /// <summary>
    ///     Controller manages API requests from the Management API
    /// </summary>
    [RoutePrefix("management")]
    public class ManagementController : ApiController
    {
        private readonly IRegiBlockchainStore _blockchainStore;
        private readonly MultiChainHandler _multichaind;

        public ManagementController(MultiChainHandler multichaind, IRegiBlockchainStore blockchainStore)
        {
            _multichaind = multichaind;
            _blockchainStore = blockchainStore;
        }

        [ApiKeyAuth]
        [HttpPost]
        [Route("createblockchain")]
        public async Task<IHttpActionResult> CreateBlockchain(CreateBlockchain model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var blockchain = new RegiBlockchain
            {
                Name = model.Name,
                ExpiryDate = model.ExpiryDate,
                ChainString = model.ChainString
            };

            await MultiChainUtilHandler.CreateBlockchain(model.ChainString);

            var blockchains = _blockchainStore.GetAllBlockchains();

            int port;
            while (true)
            {
                port = MultiChainTools.GetNewPort(EPortType.MultichainD);
                if (blockchains.All(b => b.Port != port))
                    break;
            }

            // TODO: Receive from Management
            var questions = new List<BlockchainQuestionModel>
            {
                new BlockchainQuestionModel
                {
                    Question = "Who is the best?",
                    Answers = new List<string>
                    {
                        "Elmo",
                        "THOR",
                        "Spongebob"
                    }
                }
            };

            UpdateParams(model.ChainString);

            var chain = await _multichaind.Connect(IPAddress.Loopback.ToString(), model.ChainString, port, port, false);
            await chain.WriteToStream(MultiChainTools.ROOT_STREAM_NAME, MultiChainTools.QUESTIONS_KEY, questions);

            blockchain.Port = port;
            await _blockchainStore.CreateBlockchain(blockchain);

            return Ok();
        }

        private static void UpdateParams(string chainName)
        {
            var p = MultiChainTools.GetBlockchainConfig(chainName);
            p["anyone-can-connect"] = true;
            p["anyone-can-send"] = true;
            p["anyone-can-receive"] = true;
            p["anyone-can-mine"] = true;
            MultiChainTools.WriteBlockchainConfig(chainName, p);
        }
    }
}