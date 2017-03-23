using System.Collections.Generic;
using System.Threading.Tasks;
using Management.Models;

namespace Management.Database.Interfaces
{
    public interface IManaUserStore
    {
        Task<IEnumerable<ManaUser>> GetUsers();
        Task<ManaUser> GetUserById(int id);
        Task<ManaUser> GetUserByEmail(string email);
        Task<ManaUser> CreateUser(ManaUser user);
        Task DeleteUser(int id);
        Task UpdateUser(ManaUser user);
    }
}