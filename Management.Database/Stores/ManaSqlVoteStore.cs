using System;
using System.Collections.Generic;
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
    public class ManaSqlVoteStore : SqlStore, IManaVoteStore
    {
        public ManaSqlVoteStore(string connectionString) : base(connectionString)
        {
        }

        private async Task<IEnumerable<ManaVote>> GetVoteByQuery(string query)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var result = await connection.QueryAsync(query);

                    if (!result.Any())
                        throw new RecordNotFoundException();

                    return result.Select((v) => new ManaDbVote(v).ToVote());
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not get Mana Vote");
            }
        }

        private async Task<IEnumerable<ManaVote>> GetVoteByQuery(string query, object parameters)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var result = await connection.QueryAsync(query, parameters);

                    if (!result.Any())
                        throw new RecordNotFoundException();

                    return result.Select((v) => new ManaDbVote(v).ToVote());
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not get Mana Vote");
            }
        }

        public async Task<ManaVote> GetVoteById(int id)
        {
            var vote = await GetVoteByQuery(ManagementQueries.VoteGetById, new { Id = id });
            return vote.First();
        }

        public async Task<IEnumerable<ManaVote>> GetAllVotes()
        {
            var vote = await GetVoteByQuery(ManagementQueries.VotesGetAll);
            return vote;
        }

        public async Task<IEnumerable<ManaVote>> GetVotes(bool published)
        {
            var vote = await GetVoteByQuery(ManagementQueries.VotesGetByPublished, new { Published = published });
            return vote;
        }

        public async Task<ManaVote> CreateVote(ManaVote vote)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var dbModel = new ManaDbVote(vote);
                    var result = await connection.ExecuteAsync(ManagementQueries.VoteCreate, dbModel);

                    return vote;
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not get create Mana Vote");
            }
        }

        public async Task<ManaVote> UpdateVote(ManaVote vote)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var dbModel = new ManaDbVote(vote);
                    await connection.ExecuteAsync(ManagementQueries.VoteUpdate, dbModel);

                    return vote;
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not get create Mana Vote");
            }
        }

        public async Task<int> DeleteVote(int voteId)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var result = await connection.ExecuteAsync(ManagementQueries.VoteDelete, new { Id = voteId });
                    return result;
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                if (e is RecordNotFoundException)
                    throw;
                throw new Exception("Could not get create Mana Vote");
            }
        }
    }
}