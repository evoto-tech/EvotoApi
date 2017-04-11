using System.Collections.Generic;
using System.Threading.Tasks;
using Registrar.Models;
using Registrar.Models.Request;

namespace Registrar.Database.Interfaces
{
    public interface IRegiSettingStore
    {
        Task<RegiSetting> UpdateSetting(UpdateRegiSetting settingName);
        Task<RegiSetting> GetSetting(string settingName);
        Task<IList<RegiSetting>> ListSettings();
    }
}