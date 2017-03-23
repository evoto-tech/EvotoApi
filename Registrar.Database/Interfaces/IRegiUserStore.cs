using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;

namespace Registrar.Database.Interfaces
{
    public interface IRegiUserStore
    {
        Task<RegiUser> GetUserById(int id);
        Task<RegiUser> GetUserByEmail(string email);
        Task<IEnumerable<RegiUser>> GetUsers();
        Task<RegiUser> CreateUser(RegiUser user);
        Task DeleteUser(int id);
        Task UpdateUser(RegiUser user);
        Task<IList<CustomUserField>> GetCustomUserFields();
    }
}