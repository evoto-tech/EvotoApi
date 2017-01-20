using System.Threading.Tasks;
using Registrar.Models;

namespace Registrar.Database.Interfaces
{
    public interface IRegiUserLockoutStore
    {
        Task<RegiUserLockout> GetUserInfo(int userId);
        Task UpdateUserAttempts(RegiUserLockout userInfo);
        Task UpdateUserTime(RegiUserLockout userInfo);
    }
}