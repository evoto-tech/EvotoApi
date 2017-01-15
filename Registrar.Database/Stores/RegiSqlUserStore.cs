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
    public class RegiSqlUserStore : SqlStore, IRegiUserStore
    {
        public RegiSqlUserStore(string connectionString) : base(connectionString)
        {
        }

        public async Task<RegiUser> GetUserById(int id)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var result = await connection.QueryAsync(RegistrarQueries.UserDeleteById, new {Id = id});

                    if (!result.Any())
                        throw new RecordNotFoundException();

                    var model = new RegiDbUser(result.First());
                    return model.ToUser();
                }
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
                throw new Exception("Could not get Regi User");
            }
        }

        public async Task<RegiUser> GetUserByEmail(string email)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var result = await connection.QueryAsync(RegistrarQueries.UserDeleteById, new {Email = email});

                    if (!result.Any())
                        throw new RecordNotFoundException();

                    var model = new RegiDbUser(result.First());
                    return model.ToUser();
                }
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
                throw new Exception("Could not get Regi User");
            }
        }

        public async Task<RegiUser> CreateUser(RegiUser user)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var dbModel = new RegiDbUser(user);
                    await connection.ExecuteAsync(RegistrarQueries.UserDeleteById, dbModel);

                    return user;
                }
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
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
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
                throw new Exception("Could not delete Regi User");
            }
        }
    }
}