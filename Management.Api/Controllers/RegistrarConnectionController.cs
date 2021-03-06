﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using EvotoApi.Connections;
using Management.Models.Exceptions;
using Registrar.Models.Request;

namespace EvotoApi.Controllers
{
    [RoutePrefix("regi")]
    [Authorize]
    public class RegistrarConnectionController : ApiController
    {
        [Route("users/list")]
        [HttpGet]
        public async Task<IHttpActionResult> List()
        {
            try
            {
                var users = await RegistrarConnection.GetRegistrarUsers();
                return Ok(users);
            }
            catch (RegistrarConnectionException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("users/detail/{userId:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> Details(int userId)
        {
            try
            {
                var user = await RegistrarConnection.GetRegistrarUser(userId);
                return Ok(user);
            }
            catch (RegistrarConnectionException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("account/register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(CreateRegiUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await RegistrarConnection.CreateRegistrarUser(model);
                return Ok(user);
            }
            catch (RegistrarConnectionException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("settings/custom-fields")]
        [HttpGet]
        public async Task<IHttpActionResult> SettingsCustomFieldsGet()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var fields = await RegistrarConnection.GetCustomFields();
                return Ok(fields);
            }
            catch (RegistrarConnectionException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("settings/custom-fields")]
        [HttpPost]
        public async Task<IHttpActionResult> SettingsCustomFieldsPost(IList<CreateCustomUserFieldModel> model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var fields = await RegistrarConnection.UpdateCustomFields(model);
                return Ok(fields);
            }
            catch (RegistrarConnectionException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("users/confirmEmail/{id:int}")]
        [HttpPost]
        public async Task<IHttpActionResult> ConfirmEmail(int id)
        {
            try
            {
                await RegistrarConnection.SetEmailConfirmed(id);
                return Ok();
            }
            catch (RegistrarConnectionException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("users/changePassword")]
        [HttpPost]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await RegistrarConnection.ChangePassword(model);
                return Ok();
            }
            catch (RegistrarConnectionException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("users/{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            try
            {
                await RegistrarConnection.DeleteUser(id);
                return Ok();
            }
            catch (RegistrarConnectionException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("settings/list")]
        [HttpGet]
        public async Task<IHttpActionResult> ListSettings()
        {
            try
            {
                var settings = await RegistrarConnection.ListRegistrarSettings();
                return Ok(settings);
            }
            catch (RegistrarConnectionException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("settings")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateSetting(UpdateRegiSetting model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var setting = await RegistrarConnection.UpdateRegistrarSetting(model);
                return Ok(setting);
            }
            catch (RegistrarConnectionException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("results")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Results(string chainString)
        {
            try
            {
                var results = await RegistrarConnection.GetResults(chainString);
                return Ok(results);
            }
            catch (RegistrarConnectionException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}