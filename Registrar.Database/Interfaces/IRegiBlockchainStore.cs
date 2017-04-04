using System.Collections.Generic;
using System.Threading.Tasks;
using Registrar.Models;

namespace Registrar.Database.Interfaces
{
    public interface IRegiBlockchainStore
    {
        Task<List<RegiBlockchain>> GetAllBlockchains();
        Task<RegiBlockchain> GetBlockchain(string name);
        Task<RegiBlockchain> CreateBlockchain(RegiBlockchain model);
        Task<IEnumerable<RegiBlockchain>> GetCurrentBlockchains();
    }
}