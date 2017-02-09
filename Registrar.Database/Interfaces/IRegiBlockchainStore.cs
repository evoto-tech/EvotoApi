using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Registrar.Database.Models;
using Registrar.Models;

namespace Registrar.Database.Interfaces
{
    public interface IRegiBlockchainStore
    {
        Task<RegiBlockchain> GetBlockchain(string name);
        Task<RegiBlockchain> CreateBlockchain(RegiBlockchain model);
        Task<IEnumerable<RegiBlockchain>> GetCurrentBlockchains();
    }
}