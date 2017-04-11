using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Exceptions;
using Dapper;
using Management.Database.Interfaces;
using Management.Database.Models;
using Management.Models;

namespace Management.Database.Stores
{
    public class ManaUserLockoutStore : SqlStore, IManaUserLockoutStore
    {
        public ManaUserLockoutStore(string connectionString) : base(connectionString)
        {
        }

        public async Task<ManaUserLockout> GetUserInfo(int userId)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var result =
                        await connection.QueryAsync(ManagementQueries.LockoutGetByUserId, new {UserId = userId});

                    if (!result.Any())
                        throw new RecordNotFoundException();

                    var model = new ManaDbUserLockout(result.First());
                    return model.ToUser();
                }
            }
            catch (Exception e)
            {
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not get Mana User");
            }
        }

        public async Task UpdateUserTime(ManaUserLockout userInfo)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var rows = await connection.ExecuteAsync(ManagementQueries.LockoutUpdateTime, userInfo);
                    if (rows == 0)
                        throw new RecordNotFoundException();
                }
            }
            catch (Exception e)
            {
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not delete Mana User");
            }
        }

        public async Task UpdateUserAttempts(ManaUserLockout userInfo)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var rows = await connection.ExecuteAsync(ManagementQueries.LockoutUpdateAttempts, userInfo);
                    if (rows == 0)
                        throw new RecordNotFoundException();
                }
            }
            catch (Exception e)
            {
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not delete Mana User");
            }
        }

        public async Task InsertUserTime(ManaUserLockout userInfo)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(ManagementQueries.LockoutInsertTime, userInfo);
                }
            }
            catch (Exception e)
            {
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not delete Mana User");
            }
        }

        public async Task InsertUserAttempts(ManaUserLockout userInfo)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(ManagementQueries.LockoutInsertAttempts, userInfo);
                }
            }
            catch (Exception e)
            {
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not delete Mana User");
            }
        }
    }
}