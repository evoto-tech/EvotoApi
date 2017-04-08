using System.Threading.Tasks;
using Registrar.Models.Request;
using Registrar.Models.Response;
using System.Collections.Generic;

namespace Registrar.Database.Interfaces
{
    public interface IRegiSettingStore
    {
        Task<SingleRegiSettingResponse> UpdateSetting(UpdateRegiSetting settingName);
        Task<SingleRegiSettingResponse> GetSetting(string settingName);
        Task<IList<SingleRegiSettingResponse>> ListSettings();
    }
}