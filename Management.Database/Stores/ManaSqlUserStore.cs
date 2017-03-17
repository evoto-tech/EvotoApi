﻿using System;
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
    public class ManaSqlUserStore : SqlStore, IManaUserStore
    {
        public ManaSqlUserStore(string connectionString) : base(connectionString)
        {
        }

        public async Task<ManaUser> GetUserById(int id)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var result = await connection.QueryAsync(ManagementQueries.UserGetById, new {Id = id});

                    if (!result.Any())
                        throw new RecordNotFoundException();

                    var model = new ManaDbUser(result.First());
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
                throw new Exception("Could not get Mana User");
            }
        }

        public async Task<ManaUser> GetUserByEmail(string email)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var result = await connection.QueryAsync(ManagementQueries.UserGetByEmail, new {Email = email});

                    if (!result.Any())
                        throw new RecordNotFoundException();

                    var model = new ManaDbUser(result.First());
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
                throw new Exception("Could not get Mana User");
            }
        }

        public async Task<ManaUser> CreateUser(ManaUser user)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var dbModel = new ManaDbUser(user);
                    await connection.ExecuteAsync(ManagementQueries.UserCreate, dbModel);

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
                throw new Exception("Could not get create Mana User");
            }
        }

        public async Task DeleteUser(int id)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(ManagementQueries.UserDeleteById, new {Id = id});
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not delete Mana User");
            }
        }

        public async Task UpdateUser(ManaUser user)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(ManagementQueries.UserUpdate, user);
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not delete Mana User");
            }
        }
    }
}