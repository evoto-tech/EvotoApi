using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Exceptions;
using Common.Models;
using Dapper;
using Registrar.Database.Interfaces;
using Registrar.Database.Models;

namespace Registrar.Database.Stores
{
    public class RegiSqlUserStore : SqlStore, IRegiUserStore
    {
        public RegiSqlUserStore(string connectionString) : base(connectionString)
        {
        }

        public async Task<IEnumerable<RegiUser>> GetUsers()
        {
            var users = await GetUserByQuery(RegistrarQueries.UserGetAll, new {});
            return users;
        }

        public async Task<RegiUser> GetUserById(int id)
        {
            var users = await GetUserByQuery(RegistrarQueries.UserGetById, new {Id = id});
            return users.First();
        }

        public async Task<RegiUser> GetUserByEmail(string email)
        {
            var users = await GetUserByQuery(RegistrarQueries.UserGetByEmail, new {Email = email});
            return users.First();
        }

        public async Task<RegiUser> CreateUser(RegiUser user)
        {
            try
            {
                var dbModel = new RegiDbUser(user);
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(RegistrarQueries.UserCreate, dbModel);
                    return user;
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not get create Regi User");
            }
        }

        public async Task DeleteUser(int id)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(RegistrarQueries.UserDeleteById, new {Id = id});
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

        public async Task UpdateUser(RegiUser user)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(RegistrarQueries.UserUpdate, user);
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not update Regi User");
            }
        }

        private async Task<IEnumerable<RegiUser>> GetUserByQuery(string query, object parameters)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var result = await connection.QueryAsync(query, parameters);

                    if (!result.Any())
                        throw new RecordNotFoundException();

                    return result.Select(v => new RegiDbUser(v).ToUser());
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
    }
}