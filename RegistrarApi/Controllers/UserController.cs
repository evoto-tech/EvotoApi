﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Common.Exceptions;
using Microsoft.AspNet.Identity;
using Registrar.Api.Models.Response;
using Registrar.Database.Interfaces;

namespace Registrar.Api.Controllers
{
    [RoutePrefix("users")]
    public class UserController : ApiController
    {
        private readonly IRegiUserStore _store;

        public UserController(IRegiUserStore userStore)
        {
            _store = userStore;
        }

        [HttpGet]
        [Route("list")]
        [ApiKeyAuth]
        public async Task<IHttpActionResult> List ()
        {
            try
            {
                var details = await _store.GetUsers();
                var res = details.Select((v) => new SingleRegiUserResponse(v));
                return Ok(res);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("details/{userId:int}")]
        [ApiKeyAuth]
        public async Task<IHttpActionResult> Details(int userId)
        {
            try
            {
                var details = await _store.GetUserById(userId);
                var res = new SingleRegiUserResponse(details);
                return Ok(res);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }
    }
}