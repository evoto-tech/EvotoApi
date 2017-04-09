using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Exceptions;
using Common.Models;
using Dapper;
using Registrar.Database.Interfaces;

namespace Registrar.Database.Stores
{
    public class RegiSqlRefreshTokenStore : SqlStore, IRegiRefreshTokenStore
    {
        public RegiSqlRefreshTokenStore(string connectionString) : base(connectionString)
        {
        }

        public async Task CreateRefreshToken(RefreshToken token)
        {
            try
            {
                var dbModel = new DbRefreshToken(token);
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(RegistrarQueries.RefreshTokenCreate, dbModel);
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not create Refresh Token");
            }
        }

        public async Task UpdateRefreshToken(RefreshToken token)
        {
            try
            {
                var dbModel = new DbRefreshToken(token);
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(RegistrarQueries.RefreshTokenUpdate, dbModel);
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not update Refresh Token");
            }
        }

        public async Task<RefreshToken> GetRefreshTokenForUser(int userId)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var res =
                        await connection.QueryAsync(RegistrarQueries.RefreshTokenSelectForUser, new {UserId = userId});

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
                throw new Exception("Could not get Refresh Token for user");
            }
        }

        public async Task<RefreshToken> GetRefreshToken(string token)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var res =
                        await connection.QueryAsync(RegistrarQueries.RefreshTokenSelect, new {Token = token});

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
                throw new Exception("Could not get Refresh Token");
            }
        }

        public async Task DeleteRefreshToken(string token)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(RegistrarQueries.RefreshTokenDelete, new {Token = token});
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not delete Refresh Token");
            }
        }

        public async Task DeleteTokensForUser(int userId)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(RegistrarQueries.RefreshTokenDeleteAllForUser, new { UserId = userId });
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not delete Refresh Tokens for user");
            }
        }
    }
}