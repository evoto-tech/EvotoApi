using System;
using System.Collections.Generic;
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
    public class RegiSqlBlockchainStore : SqlStore, IRegiBlockchainStore
    {
        public RegiSqlBlockchainStore(string connectionString) : base(connectionString)
        {
        }

        public async Task<List<RegiBlockchain>> GetAllBlockchains()
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var rows = await connection.QueryAsync(RegistrarQueries.BlockchainGetAll);
                    return rows.Select(r => new RegiDbBlockchain(r).ToBlockchain()).ToList();
                }
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
                throw new Exception("Could not get all Blockchains");
            }
        }

        public async Task<RegiBlockchain> CreateBlockchain(RegiBlockchain model)
        {
            try
            {
                var dbModel = new RegiDbBlockchain(model);
                using (var connection = await GetConnectionAsync())
                {
                    await connection.ExecuteAsync(RegistrarQueries.BlockchainCreate, dbModel);
                    return model;
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not create Blockchain");
            }
        }

        public async Task<IEnumerable<RegiBlockchain>> GetCurrentBlockchains()
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var rows = await connection.QueryAsync(RegistrarQueries.BlockchainsNotExpired, new {DateTime.Now});
                    return rows.Select(r => new RegiDbBlockchain(r).ToBlockchain()).ToList();
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not get current Blockchains");
            }
        }

        public async Task<RegiBlockchain> GetBlockchain(string name)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var rows = await connection.QueryAsync(RegistrarQueries.BlockchainByName, new {Name = name});
                    if (!rows.Any())
                        throw new RecordNotFoundException();

                    return new RegiDbBlockchain(rows.First()).ToBlockchain();
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not get Blockchain");
            }
        }
    }
}