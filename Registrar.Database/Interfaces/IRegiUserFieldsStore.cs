using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;

namespace Registrar.Database.Interfaces
{
    public interface IRegiUserFieldsStore
    {
        Task<IList<CustomUserField>> GetCustomUserFields();
        Task CreateCustomUserField(CustomUserField field);
        Task DeleteCustomUserField(CustomUserField field);
        Task UpdateCustomUserField(CustomUserField field);
        Task AddFieldValueForUser(RegiUser user, CustomUserValue value);
        Task DeleteValuesForUser(RegiUser user);
        Task UpdateUserView();
    }
}