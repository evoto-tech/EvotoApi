﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Common.Exceptions;
using EvotoApi.Areas.RegistrarApi.Models;
using Registrar.Database.Interfaces;

namespace EvotoApi.Areas.RegistrarApi.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IRegiUserStore _store;

        public AccountController(IRegiUserStore userStore)
        {
            _store = userStore;
        }

        [Route("details")]
        public IHttpActionResult Details(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Register new account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register", Name="RegisterAccount")]
        public async Task<IHttpActionResult> Register(WebCreateRegiUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var siteModel = model.ToModel();

            try
            {
                await _store.CreateUser(siteModel);
                return Ok();
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
                return InternalServerError();
            }
        }

        /// <summary>
        /// Login account using email and password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IHttpActionResult> Login(WebLoginRegiUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userDetails = await _store.GetUserByEmail(model.Email);

                if (Passwords.VerifyPassword(model.Password, userDetails.PasswordHash))
                {
                    return Ok();
                }
                return Unauthorized();
            }
            catch (RecordNotFoundException)
            {
                return Unauthorized();
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

        [Route("delete")]
        public IHttpActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}