using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Exceptions;
using Dapper;
using Registrar.Database.Interfaces;
using Registrar.Models.Request;
using Registrar.Models.Response;
using System.Collections.Generic;

namespace Registrar.Database.Stores
{
    public class RegiSqlSettingStore : SqlStore, IRegiSettingStore
    {
        public RegiSqlSettingStore(string connectionString) : base(connectionString)
        {
        }

        public async Task<SingleRegiSettingResponse> UpdateSetting(UpdateRegiSetting setting)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(RegistrarQueries.SettingUpdate, setting);
                    return new SingleRegiSettingResponse(setting);
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not update Regi setting");
            }
        }

        public async Task<SingleRegiSettingResponse> GetSetting(string settingName)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var result = await connection.QueryAsync(RegistrarQueries.SettingGet, new { Name = settingName });

                    if (!result.Any())
                        throw new RecordNotFoundException();

                    return new SingleRegiSettingResponse(result.First());
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not get Regi setting");
            }
        }

        public async Task<IList<SingleRegiSettingResponse>> ListSettings()
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var result = await connection.QueryAsync(RegistrarQueries.SettingGetAll);

                    if (!result.Any())
                        throw new RecordNotFoundException();

                    return result.Select((v) => new SingleRegiSettingResponse(v)).ToList();
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not get Regi settings");
            }
        }
    }
}