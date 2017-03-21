﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Common.Exceptions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Registrar.Api.Auth;
using Registrar.Api.Models.Request;
using Registrar.Api.Models.Response;
using Registrar.Database;
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
        [Authorize]
        public async Task<IHttpActionResult> List ()
        {
            var details = await _store.GetUsers();
            var res = details.Select((v) => new SingleRegiUserResponse(v));
            return Ok(res);
        }

        [HttpGet]
        [Route("details/{userId:int}")]
        [Authorize]
        public async Task<IHttpActionResult> Details(int userId)
        {
            var details = await _store.GetUserById(userId);
            var res = new SingleRegiUserResponse(details);
            return Ok(res);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }

        #endregion
    }
}