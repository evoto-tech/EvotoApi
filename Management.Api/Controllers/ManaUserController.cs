using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Management.Database.Interfaces;
using Management.Models.Response;
using EvotoApi.Auth;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Management.Models.Request;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace EvotoApi.Controllers
{
    [RoutePrefix("mana/user")]
    public class ManaUserController : ApiController
    {
        private readonly IManaUserStore _store;

        private ManaUserManager _userManager;

        public ManaUserManager UserManager
            => _userManager ?? (_userManager = HttpContext.Current.GetOwinContext().Get<ManaUserManager>());

        public ManaUserController(IManaUserStore userStore)
        {
            _store = userStore;
        }

        /// <summary>
        ///     Create a management user
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Create(CreateManaUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var user = new ManaAuthUser { Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);

            // Any errors in UserManager (such as duplicate email or insufficient password strength)
            if (!result.Succeeded)
            {
                AddErrors(result);
                return BadRequest(ModelState);
            }

            return Ok();
        }

        /// <summary>
        ///     Get a list of management users
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("list")]
        public async Task<IHttpActionResult> UserList()
        {
            var users = await _store.GetUsers();
            var response = users.Select(v => new SingleManaUserResponse(v)).ToList();
            return Ok(response);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            AddErrors(result.Errors);
        }

        private void AddErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
                ModelState.AddModelError("", error);
        }

        #endregion
    }
}