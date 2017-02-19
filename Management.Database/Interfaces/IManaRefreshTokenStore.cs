using System.Threading.Tasks;
using Common.Models;

namespace Management.Database.Interfaces
{
    public interface IManaRefreshTokenStore
    {
        Task CreateRefreshToken(RefreshToken token);
        Task UpdateRefreshToken(RefreshToken token);
        Task<RefreshToken> GetRefreshTokenForUser(int userId);
        Task<RefreshToken> GetRefreshToken(string token);
        Task DeleteRefreshToken(string token);
    }
}