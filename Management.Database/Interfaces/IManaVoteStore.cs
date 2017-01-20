using System.Collections.Generic;
using System.Threading.Tasks;
using Management.Models;

namespace Management.Database.Interfaces
{
    public interface IManaVoteStore
    {
        Task<ManaVote> GetVoteById(int id);
        Task<IEnumerable<ManaVote>> GetUserVotes(int userId, string state);
        Task<IEnumerable<ManaVote>> GetOrgVotes(int orgId, string state);
        Task<ManaVote> CreateVote(ManaVote vote);
    }
}