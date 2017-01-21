using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Common.Exceptions;
using EvotoApi.Areas.ManagementApi.Models;
using EvotoApi.Areas.ManagementApi.Models.Request;
using EvotoApi.Areas.ManagementApi.Models.Response;
using Management.Database.Interfaces;
using Management.Models;

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
        /// Get list of votes for user, needs authorize to be added
        /// </summary>
        [HttpGet]
        [Route("list/user/{state?}")]
        public async Task<IHttpActionResult> UserList(string state = "all")
        {
            if (state != "all" && state != "draft" && state != "published") state = "all";
            try
            {
                var votes = await _store.GetUserVotes(2, state);
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
    }
}