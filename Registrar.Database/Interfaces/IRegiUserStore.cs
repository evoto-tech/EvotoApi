using System.Threading.Tasks;
using Registrar.Models;

namespace Registrar.Database.Interfaces
{
    public interface IRegiUserStore
    {
        Task<RegiUser> GetUserById(int id);
        Task<RegiUser> GetUserByEmail(string email);
        Task<RegiUser> CreateUser(RegiUser user);
        Task DeleteUser(int id);
        Task UpdateUser(RegiUser user);
    }
}