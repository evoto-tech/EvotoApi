using System.Threading.Tasks;
using Registrar.Models.Request;
using Registrar.Database.Models;
using System.Collections.Generic;

namespace Registrar.Database.Interfaces
{
    public interface IRegiSettingStore
    {
        Task<RegiDbSetting> UpdateSetting(UpdateRegiSetting settingName);
        Task<RegiDbSetting> GetSetting(string settingName);
        Task<IList<RegiDbSetting>> ListSettings();
    }
}