using System.Threading.Tasks;
using Registrar.Models;

namespace Registrar.Database.Interfaces
{
    public interface IRegiUserLockoutStore
    {
        Task<RegiUserLockout> GetUserInfo(int userId);
        Task InsertUserAttempts(RegiUserLockout userInfo);
        Task UpdateUserAttempts(RegiUserLockout userInfo);
        Task InsertUserTime(RegiUserLockout userInfo);
        Task UpdateUserTime(RegiUserLockout userInfo);
        Task DeleteInfoForUser(int userId);
    }
}