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
            // Must have at least one question
            if ((model.Questions == null) || !model.Questions.Any())
            {
                ModelState.AddModelError("Questions", "At least one question is required");
                return BadRequest(ModelState);
            }

            // All questions must have at least one answer (although should probably have at least two?)
            if (model.Questions.Any(q => (q.Answers == null) || !q.Answers.Any()))
            {
                ModelState.AddModelError("Questions", "All questions must have at least one answer");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var blockchain = new RegiBlockchain
            {
                Name = model.Name,
                ExpiryDate = model.ExpiryDate,
                Info = model.Info,
                ChainString = model.ChainString
            };

            // multichain-util create {model.ChainString}
            await MultiChainUtilHandler.CreateBlockchain(model.ChainString);

            // Find a Port to run multichaind on
            var blockchains = _blockchainStore.GetAllBlockchains();

            int port;
            while (true)
            {
                port = MultiChainTools.GetNewPort(EPortType.MultichainD);
                if (blockchains.All(b => b.Port != port))
                    break;
            }

            // Update the default multichain.params to adjust permissions etc.
            UpdateParams(model.ChainString);

            // Run the blockchain for the first time
            var rpcPort = MultiChainTools.GetNewPort(EPortType.MultichainD);
            var chain =
                await _multichaind.Connect(IPAddress.Loopback.ToString(), model.ChainString, port, port, rpcPort, false);

            // Convert questions. Each has a unique number (per blockchain) starting from 1
            var questionNumber = 1;
            foreach (var q in model.Questions.Select(q => RequestToBlockchainQuestion(q, ref questionNumber)))
            {
                // Write each question as a new entry in the Root Stream unnder the Questions Key
                await chain.WriteToStream(MultiChainTools.ROOT_STREAM_NAME, MultiChainTools.QUESTIONS_KEY, q);
            }

            // Create a new wallet ID for votes to be sent to
            var walletId = await chain.GetNewWalletAddress();

            // Persist port and wallet ID in db
            blockchain.WalletId = walletId;
            blockchain.Port = port;

            await _blockchainStore.CreateBlockchain(blockchain);

            return Ok();
        }

        [Route("results")]
        [HttpGet]
        public async Task<IHttpActionResult> Results(string blockchainName)
        {
            // Get blockchain info. Ensure exists and is connected to
            var blockchain = await _blockchainStore.GetBlockchain(blockchainName);

            MultichainModel chain;
            if (!_multichaind.Connections.TryGetValue(blockchain.ChainString, out chain))
                return NotFound();

            // Get the votes, aka transactions to our wallet ID
            var votes = await chain.GetAddressTransactions(blockchain.WalletId);

            // Read the answers from hex
            var answers = votes
                .Select(v => MultiChainClient.ParseHexString(v.Data.First()))
                .Select(Encoding.UTF8.GetString)
                .Select(JsonConvert.DeserializeObject<BlockchainVoteModel>).ToList();

            // Read the questions from the blockchain
            var questions = await chain.GetQuestions();

            // For each question, get its total for each answer
            var results = questions.Select(question =>
            {
                // Setup response dictionary, answer -> num votes
                var options = question.Answers.ToDictionary(a => a.Answer, a => 0);
                foreach (var answer in answers)
                {
                    // Each vote has answer for multiple questions. Only look at the one corresponding to our current question
                    foreach (var questionAnswer in answer.Answers.Where(a => a.Question == question.Number))
                    {
                        // In case we have anything unusual going on
                        if (!options.ContainsKey(questionAnswer.Answer))
                        {
                            Debug.WriteLine($"Unexpected answer for question {questionAnswer.Question}: {questionAnswer.Answer}");
                            continue;
                        }
                        options[questionAnswer.Answer]++;
                    }
                }

                return new
                {
                    question.Number,
                    question.Question,
                    Results = options
                };
            });

            return Ok(results);
        }

        private static BlockchainQuestionModel RequestToBlockchainQuestion(CreateBlockchainQuestion q, ref int i)
        {
            return new BlockchainQuestionModel
            {
                Number = i++,
                Question = q.Question,
                Info = q.Info,
                Answers = q.Answers.Select(a => new BlockchainAnswerModel
                {
                    Answer = a.Answer,
                    Info = a.Info
                }).ToList()
            };
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