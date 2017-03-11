using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Blockchain;
using Blockchain.Models;
using Common;
using MultiChainLib.Client;
using Newtonsoft.Json;
using Registrar.Api.Models.Request;
using Registrar.Database.Interfaces;
using Registrar.Models;

namespace Registrar.Api.Controllers
{
    /// <summary>
    ///     Controller manages API requests from the Management API
    /// </summary>
    [RoutePrefix("management")]
    [ApiKeyAuth]
    public class ManagementController : ApiController
    {
        private readonly IRegiBlockchainStore _blockchainStore;
        private readonly MultiChainHandler _multichaind;

        public ManagementController(MultiChainHandler multichaind, IRegiBlockchainStore blockchainStore)
        {
            _multichaind = multichaind;
            _blockchainStore = blockchainStore;
        }

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
            var question = new BlockchainQuestionModel
            {
                Question = "Who is the best?",
                Answers = new List<string>
                {
                    "Elmo",
                    "THOR",
                    "Spongebob"
                }
            };

            UpdateParams(model.ChainString);

            var rpcPort = MultiChainTools.GetNewPort(EPortType.MultichainD);
            var chain =
                await _multichaind.Connect(IPAddress.Loopback.ToString(), model.ChainString, port, port, rpcPort, false);
            await chain.WriteToStream(MultiChainTools.ROOT_STREAM_NAME, MultiChainTools.QUESTIONS_KEY, question);

            var walletId = await chain.GetNewWalletAddress();

            blockchain.WalletId = walletId;
            blockchain.Port = port;

            await _blockchainStore.CreateBlockchain(blockchain);

            return Ok();
        }

        [Route("results")]
        [HttpGet]
        public async Task<IHttpActionResult> Results(string blockchainName)
        {
            var blockchain = await _blockchainStore.GetBlockchain(blockchainName);

            MultichainModel chain;
            if (!_multichaind.Connections.TryGetValue(blockchain.ChainString, out chain))
                return NotFound();

            var votes = await chain.GetAddressTransactions(blockchain.WalletId);

            var answers = votes
                .Select(v => MultiChainClient.ParseHexString(v.Data.First()))
                .Select(Encoding.UTF8.GetString)
                .Select(JsonConvert.DeserializeObject<BlockchainAnswerModel>)
                .ToList();


            var questions = await chain.GetQuestions();

            var options = questions.First().Answers.ToDictionary(a => a, a => 0);
            foreach (var a in answers)
            {
                if (!options.ContainsKey(a.Answer))
                {
                    Debug.WriteLine($"Unexpected answer: {a}");
                    continue;
                }
                options[a.Answer]++;
            }

            return Ok(options);
        }

        private static void UpdateParams(string chainName)
        {
            var p = MultiChainTools.GetBlockchainConfig(chainName);

            p["anyone-can-connect"] = true;
            p["anyone-can-send"] = true;
            p["anyone-can-receive"] = true;
            p["anyone-can-mine"] = true;

            p["root-stream-open"] = false;

            MultiChainTools.WriteBlockchainConfig(chainName, p);
        }
    }
}