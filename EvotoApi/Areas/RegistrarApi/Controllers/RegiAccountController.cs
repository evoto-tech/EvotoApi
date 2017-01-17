using System;
using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Common.Exceptions;
using EvotoApi.Areas.RegistrarApi.Models;
using EvotoApi.Areas.RegistrarApi.Models.Request;
using EvotoApi.Areas.RegistrarApi.Models.Response;
using Registrar.Database.Interfaces;

namespace EvotoApi.Areas.RegistrarApi.Controllers
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
        public async Task<IHttpActionResult> Register(WebCreateRegiUser model)
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

        /// <summary>
        ///     Login account using email and password
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

                if (!Passwords.VerifyPassword(model.Password, userDetails.PasswordHash))
                    return Unauthorized();

                var response = new SingleRegiUserResponse(userDetails);
                return Json(response);
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

        [HttpDelete]
        [Route("delete")]
        public IHttpActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}