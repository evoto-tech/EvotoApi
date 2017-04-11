using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Blockchain;
using Blockchain.Models;
using Common;
using Common.Exceptions;
using Registrar.Database.Interfaces;
using Registrar.Models;
using Registrar.Models.Request;
using Registrar.Models.Response;

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

            // multichain-util create {model.ChainString}
            await MultiChainUtilHandler.CreateBlockchain(model.ChainString);

            // Find a Port to run multichaind on
            var blockchains = await _blockchainStore.GetAllBlockchains();

            int port;
            while (true)
            {
                port = MultiChainTools.GetNewPort(EPortType.MultichainD);
                if (blockchains.All(b => b.Port != port))
                    break;
            }

            // Update the default multichain.params to adjust permissions etc.
            UpdateParams(model);

            // Run the blockchain for the first time
            var rpcPort = MultiChainTools.GetNewPort(EPortType.MultichainD);
            var chain =
                await _multichaind.Connect(IPAddress.Loopback.ToString(), model.ChainString, port, port, rpcPort, false);

            // Convert questions. Each has a unique number (per blockchain) starting from 1
            var questionNumber = 1;
            foreach (var q in model.Questions.Select(q => RequestToBlockchainQuestion(q, ref questionNumber)))
                await chain.WriteToStream(MultiChainTools.ROOT_STREAM_NAME, MultiChainTools.QUESTIONS_KEY, q);

            // Create a new wallet ID for votes to be sent to
            var walletId = await chain.GetNewWalletAddress();

            var blockchain = new RegiBlockchain
            {
                Name = model.Name,
                ExpiryDate = model.ExpiryDate,
                Info = model.Info,
                ChainString = model.ChainString,
                WalletId = walletId,
                Port = port
            };

            // Encrypt blockchain results
            if (model.Encrypted)
            {
                // Create encryption key TODO: Don't store on disk?
                var key = RsaTools.CreateKeyAndSave(blockchain.ChainString + "-encrypt");
                blockchain.EncryptKey = RsaTools.KeyToString(key.Public);
            }

            // Save blockchain data in store
            await _blockchainStore.CreateBlockchain(blockchain);

            // Create RSA keypair for blind signing
            RsaTools.CreateKeyAndSave(blockchain.ChainString);

            return Ok();
        }

        [Route("results")]
        [HttpGet]
        public async Task<IHttpActionResult> Results(string blockchainName)
        {
            RegiBlockchain blockchain;
            try
            {
                // Get blockchain info. Ensure exists and is connected to
                blockchain = await _blockchainStore.GetBlockchain(blockchainName);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }

            MultichainModel chain;
            if (!_multichaind.Connections.TryGetValue(blockchain.ChainString, out chain))
                return NotFound();

            var priv = "";
            if (!string.IsNullOrWhiteSpace(blockchain.EncryptKey))
            {
                var key = RsaTools.LoadKeysFromFile(blockchain.ChainString + "-encrypt");
                priv = RsaTools.KeyToString(key.Private);
            }

            var answers = await chain.GetResults(blockchain.WalletId, priv);

            // Read the questions from the blockchain
            var questions = await chain.GetQuestions();

            // For each question, get its total for each answer
            var results = questions.Select(question =>
            {
                // Setup response dictionary, answer -> num votes
                var options = question.Answers.ToDictionary(a => a.Answer, a => 0);
                foreach (var answer in answers)
                {
                    // Look for the answer number matching our question
                    foreach (var questionAnswer in answer.Answers.Where(a => a.Question == question.Number))
                    {
                        // In case we have anything unusual going on
                        if (!options.ContainsKey(questionAnswer.Answer))
                        {
                            Debug.WriteLine(
                                $"Unexpected answer for question {questionAnswer.Question}: {questionAnswer.Answer}");
                            continue;
                        }
                        options[questionAnswer.Answer]++;
                    }
                }

                return new BlockchainQuestionResultsResponse(
                    question.Number,
                    question.Question,
                    options
                );
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

        private static void UpdateParams(CreateBlockchain model)
        {
            var p = MultiChainTools.GetBlockchainConfig(model.ChainString);

            p["anyone-can-connect"] = true;
            p["anyone-can-send"] = true;
            p["anyone-can-receive"] = true;
            p["anyone-can-mine"] = true;

            p["root-stream-open"] = false;

            // Target block time must be between 5 and 86400 seconds
            if ((model.BlockSpeed >= 5) && (model.BlockSpeed <= 86400))
                p["target-block-time"] = model.BlockSpeed;

            MultiChainTools.WriteBlockchainConfig(model.ChainString, p);
        }
    }
}