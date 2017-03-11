using System.Threading.Tasks;
using System.Web.Http;
using Registrar.Database.Interfaces;

namespace Registrar.Api.Controllers
{
    [RoutePrefix("home")]
    [Authorize]
    public class HomeController : ApiController
    {
        private readonly IRegiBlockchainStore _blockchainStore;

        public HomeController(IRegiBlockchainStore blockchainStore)
        {
            _blockchainStore = blockchainStore;
        }

        [Route("votes")]
        [HttpGet]
        public async Task<IHttpActionResult> GetVotes()
        {
            var votes = await _blockchainStore.GetCurrentBlockchains();
            return Ok(votes);
        }
    }
}