using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Common.Exceptions;
using EvotoApi.Connections;
using Management.Database.Interfaces;
using Management.Models;
using Management.Models.Request;
using Management.Models.Response;
using Microsoft.AspNet.Identity;

namespace EvotoApi.Controllers
{
    [RoutePrefix("mana/vote")]
    [Authorize]
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
                var created = await RegistrarConnection.CreateBlockchain(vote);
                if (created)
                    return true;
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
            return true;
        }

        /// <summary>
        ///     Get a vote by its id
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
        ///     Get list of all votes for the current user
        /// </summary>
        [HttpGet]
        [Route("list")]
        public async Task<IHttpActionResult> List()
        {
            try
            {
                var votes = await _store.GetAllVotes();
                var response = votes.Select(v => new ManaVoteResponse(v)).ToList();
                return Json(response);
            }
            catch (RecordNotFoundException)
            {
                return Json(new object[] {});
            }
        }

        /// <summary>
        ///     Get list of votes for the current user based on published
        /// </summary>
        [HttpGet]
        [Route("list/{published:bool}")]
        public async Task<IHttpActionResult> List(bool published)
        {
            try
            {
                var votes = await _store.GetVotes(published);
                var response = votes.Select(v => new ManaVoteResponse(v)).ToList();
                return Json(response);
            }
            catch (RecordNotFoundException)
            {
                return Json(new object[] {});
            }
        }

        /// <summary>
        ///     Create a vote
        /// </summary>
        [HttpPost]
        [Route("create")]
        public async Task<IHttpActionResult> VoteCreate(CreateManaVote model)
        {
            model.CreatedBy = User.Identity.GetUserId<int>();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var voteModel = model.ToModel();

            try
            {
                if (voteModel.Published) voteModel.PublishedDate = DateTime.Now;
                var vote = await _store.CreateVote(voteModel);
                var response = new ManaVoteResponse(vote);
                var publishStateValid = await CheckAndPublish(vote);
                if (!publishStateValid)
                    return Json(new
                    {
                        errors = "Your changes have been saved but there was an issue publishing this vote."
                    });
                return Json(response);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        /// <summary>
        ///     Edit a vote
        /// </summary>
        [HttpPatch]
        [Route("{voteId:int}/edit")]
        public async Task<IHttpActionResult> VoteEdit(int voteId, CreateManaVote model)
        {
            if (model.Published) model.PublishedDate = DateTime.Now;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var voteModel = model.ToModel();
            voteModel.Id = voteId;
            try
            {
                var updatedVote = await _store.UpdateVote(voteModel);
                var response = new ManaVoteResponse(updatedVote);
                var publishStateValid = await CheckAndPublish(updatedVote);
                if (!publishStateValid)
                    return Json(new
                    {
                        errors = "Your changes have been saved but there was an issue publishing this vote."
                    });
                return Json(response);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        /// <summary>
        ///     Delete a vote
        /// </summary>
        [HttpDelete]
        [Route("{voteId:int}/delete")]
        public async Task<IHttpActionResult> VoteDelete(int voteId)
        {
            try
            {
                var affectedVote = await _store.GetVoteById(voteId);
                if (!affectedVote.Published)
                {
                    var affectedRows = await _store.DeleteVote(voteId);
                    return Json(affectedRows);
                }
                return new BadRequestErrorMessageResult(
                    "Published votes cannot be deleted", this);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}