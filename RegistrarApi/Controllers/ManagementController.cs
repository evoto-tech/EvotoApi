using System;
using System.Threading.Tasks;
using System.Web.Http;
using Blockchain;
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
        private readonly IMultiChainHandler _multichaind;
        private readonly IRegiBlockchainStore _blockchainStore;

        public ManagementController(IMultiChainHandler multichaind, IRegiBlockchainStore blockchainStore)
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

            try
            {
                await MultiChainUtilHandler.CreateBlockchain(model.ChainString);
                await _multichaind.Connect();
                await _blockchainStore.CreateBlockchain(blockchain);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return InternalServerError();
            }
        }
    }
}