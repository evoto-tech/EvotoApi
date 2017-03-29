﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Common.Exceptions;
using EvotoApi.Areas.ManagementApi.Models.Response;
using Management.Database.Interfaces;

namespace EvotoApi.Areas.Management.Controllers
{
    [RoutePrefix("mana/user")]
    public class ManaUserController : ApiController
    {
        private readonly IManaUserStore _store;

        public ManaUserController(IManaUserStore userStore)
        {
            _store = userStore;
        }

        /// <summary>
        /// Get a list of management users
        /// </summary>
        [HttpGet]
        //TODO: [Authorize]
        [Route("list")]
        public async Task<IHttpActionResult> UserList()
        {
            var users = await _store.GetUsers();
            var response = users.Select((v) => new SingleManaUserResponse(v)).ToList();
            return Json(response);
        }
    }
}