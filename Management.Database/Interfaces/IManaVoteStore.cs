using System.Collections.Generic;
using System.Threading.Tasks;
using Management.Models;

namespace Management.Database.Interfaces
{
    public interface IManaVoteStore
    {
        Task<ManaVote> GetVoteById(int id);
        Task<IEnumerable<ManaVote>> GetAllVotes();
        Task<IEnumerable<ManaVote>> GetVotes(bool published);
        Task<ManaVote> CreateVote(ManaVote vote);
        Task<ManaVote> UpdateVote(ManaVote vote);
        Task DeleteVote(int id);
    }
}