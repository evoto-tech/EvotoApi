using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Exceptions;
using Common.Models;
using Dapper;
using Management.Database.Interfaces;

namespace Management.Database.Stores
{
    public class ManaSqlRefreshTokenStore : SqlStore, IManaRefreshTokenStore
    {
        public ManaSqlRefreshTokenStore(string connectionString) : base(connectionString)
        {
        }

        public async Task CreateRefreshToken(RefreshToken token)
        {
            try
            {
                var dbModel = new DbRefreshToken(token);
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(ManagementQueries.RefreshTokenCreate, dbModel);
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

        public async Task UpdateRefreshToken(RefreshToken token)
        {
            try
            {
                var dbModel = new DbRefreshToken(token);
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(ManagementQueries.RefreshTokenUpdate, dbModel);
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

        public async Task<RefreshToken> GetRefreshTokenForUser(int userId)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var res =
                        await connection.QueryAsync(ManagementQueries.RefreshTokenSelectForUser, new {UserId = userId});

                    if (!res.Any())
                        throw new RecordNotFoundException();

                    var dbModel = new DbRefreshToken(res.First());
                    return dbModel.ToModel();
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

        public async Task<RefreshToken> GetRefreshToken(string token)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var res =
                        await connection.QueryAsync(ManagementQueries.RefreshTokenSelect, new {Token = token});

                    if (!res.Any())
                        throw new RecordNotFoundException();

                    var dbModel = new DbRefreshToken(res.First());
                    return dbModel.ToModel();
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

        public async Task DeleteRefreshToken(string token)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(ManagementQueries.RefreshTokenDelete, new {Token = token});
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