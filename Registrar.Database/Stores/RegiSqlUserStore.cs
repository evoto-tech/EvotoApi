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
                    var result = await connection.QueryAsync(RegistrarQueries.UserGetById, new {Id = id});

                    if (!result.Any())
                        throw new RecordNotFoundException();

                    var model = new RegiDbUser(result.First());
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

        public async Task<RegiUser> GetUserByEmail(string email)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var result = await connection.QueryAsync(RegistrarQueries.UserGetByEmail, new {Email = email});

                    if (!result.Any())
                        throw new RecordNotFoundException();

                    var model = new RegiDbUser(result.First());
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
                throw new Exception("Could not delete Regi User");
            }
        }
    }
}