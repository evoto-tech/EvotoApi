using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Blockchain;
using Registrar.Database.Interfaces;
using Registrar.Models.Response;

namespace Registrar.Api.Controllers
{
    [RoutePrefix("home")]
    [Authorize]
    public class HomeController : ApiController
    {
        private readonly IRegiBlockchainStore _blockchainStore;
        private readonly MultiChainHandler _multichaind;

        public HomeController(IRegiBlockchainStore blockchainStore, MultiChainHandler multichain)
        {
            _blockchainStore = blockchainStore;
            _multichaind = multichain;
        }

        [Route("votes")]
        [HttpGet]
        public async Task<IHttpActionResult> GetVotes()
        {
            var votes = await _blockchainStore.GetCurrentBlockchains();

            var response = new List<SingleBlockchainResponse>();
            foreach (var vote in votes)
            {
                // TODO: Error handle
                var info = await _multichaind.Connections[vote.ChainString].RpcClient.GetInfoAsync();
                response.Add(new SingleBlockchainResponse(vote, info.Result.Blocks));
            }

            return Ok(response);
        }
    }
}