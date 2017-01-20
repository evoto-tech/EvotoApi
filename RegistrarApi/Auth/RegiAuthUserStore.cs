using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Registrar.Database.Interfaces;
using Registrar.Models;

namespace Registrar.Api.Auth
{
    public class RegiAuthUserStore : IUserStore<RegiAuthUser>
    {
        private readonly IRegiUserStore _store;

        public RegiAuthUserStore(IRegiUserStore store)
        {
            _store = store;
        }

        public void Dispose()
        {
        }

        public async Task CreateAsync(RegiAuthUser user)
        {
            await _store.CreateUser(user);
        }

        public async Task UpdateAsync(RegiAuthUser user)
        {
            await _store.UpdateUser(user);
        }

        public async Task DeleteAsync(RegiAuthUser user)
        {
            await _store.DeleteUser(Convert.ToInt32(user.Id));
        }

        public async Task<RegiAuthUser> FindByIdAsync(string userId)
        {
            var user = await _store.GetUserById(Convert.ToInt32(userId));
            return new RegiAuthUser(user);
        }

        public async Task<RegiAuthUser> FindByNameAsync(string userName)
        {
            var user = await _store.GetUserByEmail(userName);
            return new RegiAuthUser(user);
        }
    }
}