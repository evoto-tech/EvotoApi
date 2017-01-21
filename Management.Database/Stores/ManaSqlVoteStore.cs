using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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

        public async Task<IEnumerable<ManaVote>> GetUserVotes(int userId, string state = "all")
        {
            dynamic vote;
            if (state == "all")
            {
                vote = await GetVoteByQuery(ManagementQueries.VotesGetByUser, new {UserId = userId});
            }
            else
            {
                vote = await GetVoteByQuery(ManagementQueries.VotesGetByUserAndState, new { UserId = userId, State = state });
            }
            return vote;
        }

        public async Task<ManaVote> CreateVote(ManaVote vote)
        {
            try
            {
                using (var connection = await GetConnectionAsync())
                {
                    var dbModel = new ManaDbVote(vote);
                    await connection.ExecuteAsync(ManagementQueries.VoteCreate, dbModel);

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
    }
}