using System;
using System.Threading.Tasks;
using Common.Exceptions;
using Microsoft.AspNet.Identity;
using Registrar.Database.Interfaces;

namespace Registrar.Api.Auth
{
    public class RegiAuthUserStore : IUserStore<RegiAuthUser, int>, IUserPasswordStore<RegiAuthUser, int>,
        IUserEmailStore<RegiAuthUser, int>, IUserLockoutStore<RegiAuthUser, int>
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

        public async Task<RegiAuthUser> FindByIdAsync(int userId)
        {
            try
            {
                var user = await _store.GetUserById(userId);
                return new RegiAuthUser(user);
            }
            catch (RecordNotFoundException)
            {
                return null;
            }
            
        }

        public async Task<RegiAuthUser> FindByNameAsync(string userName)
        {
            try
            {
                var user = await _store.GetUserByEmail(userName);
                return new RegiAuthUser(user);
            }
            catch (RecordNotFoundException)
            {
                return null;
            }
        }

        public async Task SetPasswordHashAsync(RegiAuthUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            await UpdateAsync(user);
        }

        public async Task<string> GetPasswordHashAsync(RegiAuthUser user)
        {
            var model = await FindByIdAsync(user.Id);
            return model?.PasswordHash;
        }

        public Task<bool> HasPasswordAsync(RegiAuthUser user)
        {
            return Task.FromResult(true);
        }

        public async Task SetEmailAsync(RegiAuthUser user, string email)
        {
            user.Email = email;
            await _store.UpdateUser(user);
        }

        public Task<string> GetEmailAsync(RegiAuthUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(RegiAuthUser user)
        {
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(RegiAuthUser user, bool confirmed)
        {
            return Task.CompletedTask;
        }

        public async Task<RegiAuthUser> FindByEmailAsync(string email)
        {
            return await FindByNameAsync(email);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(RegiAuthUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(RegiAuthUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(RegiAuthUser user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(RegiAuthUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(RegiAuthUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetLockoutEnabledAsync(RegiAuthUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(RegiAuthUser user, bool enabled)
        {
            throw new NotImplementedException();
        }
    }
}