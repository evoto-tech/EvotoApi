using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Exceptions;
using Dapper;
using Registrar.Database.Interfaces;
using Registrar.Database.Models;
using Registrar.Models;

namespace Registrar.Database.Stores
{
    public class RegiUserLockoutStore : SqlStore, IRegiUserLockoutStore
    {
        public RegiUserLockoutStore(string connectionString) : base(connectionString)
        {
        }

        public async Task<RegiUserLockout> GetUserInfo(int userId)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var result = await connection.QueryAsync(RegistrarQueries.LockoutGetByUserId, new {UserId = userId});

                    if (!result.Any())
                        throw new RecordNotFoundException();

                    var model = new RegiDbUserLockout(result.First());
                    return model.ToUser();
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not get Regi User");
            }
        }

        public async Task UpdateUserTime(RegiUserLockout userInfo)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var rows = await connection.ExecuteAsync(RegistrarQueries.LockoutUpdateTime, userInfo);
                    if (rows == 0)
                        throw new RecordNotFoundException();
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not delete Regi User");
            }
        }

        public async Task UpdateUserAttempts(RegiUserLockout userInfo)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var rows = await connection.ExecuteAsync(RegistrarQueries.LockoutUpdateAttempts, userInfo);
                    if (rows == 0)
                        throw new RecordNotFoundException();
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not delete Regi User");
            }
        }

        public async Task InsertUserTime(RegiUserLockout userInfo)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(RegistrarQueries.LockoutInsertTime, userInfo);
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not delete Regi User");
            }
        }

        public async Task InsertUserAttempts(RegiUserLockout userInfo)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(RegistrarQueries.LockoutInsertAttempts, userInfo);
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not delete Regi User");
            }
        }
    }
}