using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
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
            if (!vote.Published)
                return true;

            var created = await RegistrarConnection.CreateBlockchain(vote);
            if (created)
                return true;

            try
            {
                vote.Published = false;
                vote.PublishedDate = null;
                await _store.UpdateVote(vote);
            }
            catch (Exception)
            {
                return false;
            }
            return false;
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
                return Ok(response);
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
                return Ok(response);
            }
            catch (RecordNotFoundException)
            {
                return Ok(new List<ManaVoteResponse>(0));
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
                return Ok(response);
            }
            catch (RecordNotFoundException)
            {
                return Ok(new List<ManaVoteResponse>(0));
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

            if (voteModel.Published)
                voteModel.PublishedDate = DateTime.UtcNow;

            var vote = await _store.CreateVote(voteModel);

            var publishStateValid = await CheckAndPublish(vote);
            if (!publishStateValid)
                return BadRequest("Your changes have been saved but there was an issue publishing this vote.");

            var response = new ManaVoteResponse(vote);
            return Ok(response);
        }

        /// <summary>
        ///     Edit a vote
        /// </summary>
        [HttpPatch]
        [Route("{voteId:int}/edit")]
        public async Task<IHttpActionResult> VoteEdit(int voteId, CreateManaVote model)
        {
            if (model.Published)
                model.PublishedDate = DateTime.UtcNow;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var voteModel = model.ToModel();
            voteModel.Id = voteId;
            try
            {
                var updatedVote = await _store.UpdateVote(voteModel);

                var publishStateValid = await CheckAndPublish(updatedVote);
                if (!publishStateValid)
                    return BadRequest("Your changes have been saved but there was an issue publishing this vote.");

                var response = new ManaVoteResponse(updatedVote);
                return Ok(response);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
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
                    await _store.DeleteVote(voteId);
                    return Ok();
                }
                return BadRequest("Published votes cannot be deleted");
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }
    }
}