using System.Threading.Tasks;
using Management.Models;

namespace Management.Database.Interfaces
{
    public interface IManaUserLockoutStore
    {
        Task<ManaUserLockout> GetUserInfo(int userId);
        Task InsertUserAttempts(ManaUserLockout userInfo);
        Task UpdateUserAttempts(ManaUserLockout userInfo);
        Task InsertUserTime(ManaUserLockout userInfo);
        Task UpdateUserTime(ManaUserLockout userInfo);
    }
}