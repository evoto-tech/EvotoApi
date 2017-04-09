using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Common.Exceptions;
using EvotoApi.Areas.Management.Connections;
using EvotoApi.Areas.ManagementApi.Models;
using EvotoApi.Areas.ManagementApi.Models.Request;
using EvotoApi.Areas.ManagementApi.Models.Response;
using Management.Database.Interfaces;
using Management.Models;
using Newtonsoft.Json;
using RestSharp;

namespace EvotoApi.Areas.ManagementApi.Controllers
{
    [RoutePrefix("mana/vote")]
    public class ManaVotesController : ApiController
    {
        private readonly IManaVoteStore _store;

        public ManaVotesController(IManaVoteStore voteStore)
        {
            _store = voteStore;
        }

        private async Task<bool> CheckAndPublish(ManaVote vote)
        {
            if (vote.Published)
            {
                vote.PublishedDate = DateTime.Now;
                var created = await RegistrarConnection.CreateBlockchain(vote);
                if (created)
                {
                    return true;
                }
                else
                {
                    try
                    {
                        vote.Published = false;
                        vote.PublishedDate = null;
                        await _store.UpdateVote(vote);
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Get a vote by its id, needs authorize to be added
        /// </summary>
        [HttpGet]
        [Route("{voteId:int}")]
        public async Task<IHttpActionResult> VoteDetails(int voteId)
        {
            try
            {
                var vote = await _store.GetVoteById(voteId);
                var response = new ManaVoteResponse(vote);
                return Json(response);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get list of all votes for user, needs authorize to be added
        /// </summary>
        [HttpGet]
        [Route("list/user/{userId:int}")]
        public async Task<IHttpActionResult> UserList(int userId)
        {
            try
            {
                var votes = await _store.GetAllUserVotes(userId);
                var response = votes.Select((v) => new ManaVoteResponse(v)).ToList();
                return Json(response);
            }
            catch (RecordNotFoundException)
            {
                return Json(new object[] { });
            }
        }

        /// <summary>
        /// Get list of votes for user based on published, needs authorize to be added
        /// </summary>
        [HttpGet]
        [Route("list/user/{userId:int}/{published:bool}")]
        public async Task<IHttpActionResult> UserList(int userId, bool published)
        {
            try
            {
                var votes = await _store.GetUserVotes(userId, published);
                var response = votes.Select((v) => new ManaVoteResponse(v)).ToList();
                return Json(response);
            }
            catch (RecordNotFoundException)
            {
                return Json(new object[] {});
            }
        }

        /// <summary>
        /// Create a vote, needs authorize to be added
        /// </summary>
        [HttpPost]
        [Route("create")]
        public async Task<IHttpActionResult> VoteCreate(CreateManaVote model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var voteModel = model.ToModel();

            try
            {
                var vote = await _store.CreateVote(voteModel);
                var response = new ManaVoteResponse(vote);
                var publishStateValid = await CheckAndPublish(vote);
                if (!publishStateValid) return Json(new
                {
                    errors = "Your changes have been saved but there was an issue publishing this vote."
                });
                return Json(response);
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                return InternalServerError();
            }
        }

        /// <summary>
        /// Edit a vote, needs authorize to be added
        /// </summary>
        [HttpPatch]
        [Route("{voteId:int}/edit")]
        public async Task<IHttpActionResult> VoteEdit(int voteId, CreateManaVote model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var voteModel = model.ToModel();
            voteModel.Id = voteId;
            try
            {
                var updatedVote = await _store.UpdateVote(voteModel);
                var response = new ManaVoteResponse(updatedVote);
                var publishStateValid = await CheckAndPublish(updatedVote);
                if (!publishStateValid) return Json(new
                {
                    errors = "Your changes have been saved but there was an issue publishing this vote."
                });
                return Json(response);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                return InternalServerError();
            }
        }

        /// <summary>
        /// Delete a vote, needs authorize to be added
        /// </summary>
        [HttpDelete]
        [Route("{voteId:int}/delete")]
        public async Task<IHttpActionResult> VoteDelete(int voteId)
        {
            try
            {
                var affectedVote = await _store.GetVoteById(voteId);
                if (affectedVote.Published)
                {
                    var affectedRows = await _store.DeleteVote(voteId);
                    return Json(affectedRows);
                }
                else
                {
                    return new System.Web.Http.Results.BadRequestErrorMessageResult(
                        "Published votes cannot be deleted", this);
                }
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                return InternalServerError();
            }
        }
    }
}