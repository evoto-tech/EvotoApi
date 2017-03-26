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

        private async Task<IEnumerable<RegiUser>> GetUserByQuery(string query, object parameters = null)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var result = await connection.QueryAsync(query, parameters);

                    if (!result.Any())
                        throw new RecordNotFoundException();

                    return result.Select((v) => new RegiDbUser(v).ToUser());
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

        public async Task<IEnumerable<RegiUser>> GetUsers()
        {
            try
            {
                var users = await GetUserByQuery(RegistrarQueries.UsersAll);
                return users;
            }
            catch (RecordNotFoundException)
            {
                return new List<RegiUser>(0);
            }
        }

        public async Task<RegiUser> GetUserById(int id)
        {
            var users = await GetUserByQuery(RegistrarQueries.UserById, new {Id = id});
            return users.First();
        }

        public async Task<RegiUser> GetUserByEmail(string email)
        {
            var users = await GetUserByQuery(RegistrarQueries.UserByEmail, new {Email = email});
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
    }
}