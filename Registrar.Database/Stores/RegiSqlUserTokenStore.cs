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
    public class RegiSqlUserTokenStore : SqlStore, IRegiUserTokenStore
    {
        public RegiSqlUserTokenStore(string connectionString) : base(connectionString)
        {
        }

        public async Task CreateUserToken(UserToken token)
        {
            try
            {
                var dbModel = new DbUserToken(token);
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(RegistrarQueries.UserTokenCreate, dbModel);
                }
            }
            catch (Exception e)
            {
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not create user token");
            }
        }

        public async Task<UserToken> GetRefreshTokenForUser(string purpose, int userId)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var res =
                        await
                            connection.QueryAsync(RegistrarQueries.UserTokenGetByPurpose,
                                new {UserId = userId, Purpose = purpose});

                    if (!res.Any())
                        throw new RecordNotFoundException();

                    var dbModel = new DbUserToken(res.First());
                    return dbModel.ToToken();
                }
            }
            catch (Exception e)
            {
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not get user token");
            }
        }

        public async Task DeleteUserToken(UserToken token)
        {
            try
            {
                var dbModel = new DbUserToken(token);
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(RegistrarQueries.UserTokenDelete,
                        dbModel);
                }
            }
            catch (Exception e)
            {
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not delete user token");
            }
        }

        public async Task UpdateUserToken(UserToken token)
        {
            try
            {
                var dbModel = new DbUserToken(token);
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(RegistrarQueries.UserTokenUpdate, dbModel);
                }
            }
            catch (Exception e)
            {
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not update refresh token");
            }
        }
    }
}