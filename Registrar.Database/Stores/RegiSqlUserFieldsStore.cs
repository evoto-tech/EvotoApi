using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Models;
using Dapper;
using Registrar.Database.Interfaces;
using Registrar.Database.Models;
using Registrar.Models;

namespace Registrar.Database.Stores
{
    public class RegiSqlUserFieldsStore : SqlStore, IRegiUserFieldsStore
    {
        public RegiSqlUserFieldsStore(string connectionString) : base(connectionString)
        {
        }

        public async Task<IList<CustomUserField>> GetCustomUserFields()
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var rows = await connection.QueryAsync(RegistrarQueries.CustomUserFieldGetAll);
                    return rows.Select(r => new DbCustomUserField(r).ToModel()).ToList();
                }
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
                throw new Exception("Could not get Custom User Fields");
            }
        }

        public async Task CreateCustomUserField(CustomUserField field)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var model = new DbCustomUserField(field);
                    await connection.ExecuteAsync(RegistrarQueries.CustomUserFieldCreate, model);
                }
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
                throw new Exception("Could not create custom user field");
            }
        }

        public async Task DeleteCustomUserField(CustomUserField field)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var model = new DbCustomUserField(field);
                    await
                        connection.ExecuteAsync(RegistrarQueries.CustomUserFieldClearValuesForField,
                            new {FieldId = model.Id});
                    await connection.ExecuteAsync(RegistrarQueries.CustomUserFieldDelete, model);
                }
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
                throw new Exception("Could not delete custom user field");
            }
        }

        public async Task UpdateCustomUserField(CustomUserField field)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var model = new DbCustomUserField(field);
                    await connection.ExecuteAsync(RegistrarQueries.CustomUserFieldUpdate, model);
                }
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
                throw new Exception("Could not delete custom user field");
            }
        }

        public async Task AddFieldValueForUser(RegiUser user, CustomUserValue value)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var model = new DbCustomUserValueIn(user, value);
                    await connection.ExecuteAsync(RegistrarQueries.CustomUserValueInsert, model);
                }
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
                throw new Exception("Could not insert custom user value");
            }
        }

        public async Task DeleteValuesForUser(RegiUser user)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    await
                        connection.ExecuteAsync(RegistrarQueries.CustomUserValueDeleteAllForUser, new {UserId = user.Id});
                }
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
                throw new Exception("Could not delete custom user values");
            }
        }

        public async Task UpdateUserView()
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var tx = connection.BeginTransaction();
                    await connection.ExecuteAsync(RegistrarQueries.UserDeleteView, transaction: tx);
                    await connection.ExecuteAsync(RegistrarQueries.UserCreateView, transaction: tx);
                    tx.Commit();
                }
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
                throw new Exception("Could not update user view");
            }
        }
    }
}