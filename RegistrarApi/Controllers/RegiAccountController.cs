﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Common.Exceptions;
using Registrar.Api.Models.Request;
using Registrar.Api.Models.Response;
using Registrar.Database.Interfaces;

namespace Registrar.Api.Controllers
{
    [RoutePrefix("regi")]
    public class RegiAccountController : ApiController
    {
        private readonly IRegiUserStore _store;

        public RegiAccountController(IRegiUserStore userStore)
        {
            _store = userStore;
        }

        /// <summary>
        /// Get details for an account by userId. Used for testing right now.
        /// </summary>
        [HttpGet]
        [Route("details/{userId:int}")]
        public async Task<IHttpActionResult> Details(int userId)
        {
            try
            {
                var user = await _store.GetUserById(userId);
                var response = new SingleRegiUserResponse(user);
                return Json(response);
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Register new account
        /// </summary>
        [HttpPost]
        [Route("register", Name = "RegisterAccount")]
        public async Task<IHttpActionResult> Register(CreateRegiUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var siteModel = model.ToModel();

            try
            {
                var user = await _store.CreateUser(siteModel);
                var response = new SingleRegiUserResponse(user);
                return Json(response);
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("edit")]
        public IHttpActionResult Edit(int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("delete")]
        public IHttpActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}