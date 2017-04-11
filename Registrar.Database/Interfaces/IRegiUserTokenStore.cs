using System.Threading.Tasks;
using Common.Models;
using Registrar.Models;

namespace Registrar.Database.Interfaces
{
    public interface IRegiUserTokenStore
    {
        Task CreateUserToken(UserToken token);
        Task UpdateUserToken(UserToken token);
        Task<UserToken> GetRefreshTokenForUser(string purpose, int userId);
        Task DeleteUserToken(UserToken token);
    }
}