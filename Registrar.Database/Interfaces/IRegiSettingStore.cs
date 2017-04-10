using System.Threading.Tasks;
using Registrar.Models.Request;
using System.Collections.Generic;
using Registrar.Models;

namespace Registrar.Database.Interfaces
{
    public interface IRegiSettingStore
    {
        Task<RegiSetting> UpdateSetting(UpdateRegiSetting settingName);
        Task<RegiSetting> GetSetting(string settingName);
        Task<IList<RegiSetting>> ListSettings();
    }
}