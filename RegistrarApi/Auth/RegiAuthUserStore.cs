using System;
using System.Threading.Tasks;
using Common.Exceptions;
using Microsoft.AspNet.Identity;
using Registrar.Database.Interfaces;
using Registrar.Models;

namespace Registrar.Api.Auth
{
    public class RegiAuthUserStore : IUserPasswordStore<RegiAuthUser, int>,
        IUserEmailStore<RegiAuthUser, int>, IUserLockoutStore<RegiAuthUser, int>, IUserTwoFactorStore<RegiAuthUser, int>
    {
        private readonly IRegiUserFieldsStore _fieldStore;
        private readonly IRegiUserLockoutStore _lockoutStore;
        private readonly IRegiUserStore _store;
        private readonly IRegiRefreshTokenStore _tokenStore;

        public RegiAuthUserStore(IRegiUserStore store, IRegiUserLockoutStore lockoutStore,
            IRegiUserFieldsStore fieldStore, IRegiRefreshTokenStore tokenStore)
        {
            _store = store;
            _lockoutStore = lockoutStore;
            _fieldStore = fieldStore;
            _tokenStore = tokenStore;
        }

        public void Dispose()
        {
        }

        #region Emails

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
            return Task.FromResult(user.EmailConfirmed);
        }

        public async Task SetEmailConfirmedAsync(RegiAuthUser user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            await _store.UpdateUser(user);
        }

        public async Task<RegiAuthUser> FindByEmailAsync(string email)
        {
            return await FindByNameAsync(email);
        }

        #endregion

        #region Lockout

        public async Task<DateTimeOffset> GetLockoutEndDateAsync(RegiAuthUser user)
        {
            try
            {
                var info = await _lockoutStore.GetUserInfo(user.Id);
                return info.LockEnd;
            }
            catch (RecordNotFoundException)
            {
                return DateTimeOffset.MinValue;
            }
        }

        public async Task SetLockoutEndDateAsync(RegiAuthUser user, DateTimeOffset lockoutEnd)
        {
            var info = new RegiUserLockout {LockEnd = lockoutEnd.DateTime, UserId = user.Id};
            try
            {
                await _lockoutStore.UpdateUserTime(info);
            }
            catch (RecordNotFoundException)
            {
                await _lockoutStore.InsertUserTime(info);
            }
        }

        public async Task<int> IncrementAccessFailedCountAsync(RegiAuthUser user)
        {
            RegiUserLockout info;
            try
            {
                info = await _lockoutStore.GetUserInfo(user.Id);
                info.Attempts++;
                await _lockoutStore.UpdateUserAttempts(info);
            }
            catch (RecordNotFoundException)
            {
                info = new RegiUserLockout
                {
                    UserId = user.Id,
                    Attempts = 1
                };
                await _lockoutStore.InsertUserAttempts(info);
            }

            return info.Attempts;
        }

        public async Task ResetAccessFailedCountAsync(RegiAuthUser user)
        {
            try
            {
                var info = await _lockoutStore.GetUserInfo(user.Id);
                info.Attempts = 0;
                await _lockoutStore.UpdateUserAttempts(info);
            }
            catch (RecordNotFoundException)
            {
                // No info stored on user, nothing to do here
            }
        }

        public async Task<int> GetAccessFailedCountAsync(RegiAuthUser user)
        {
            try
            {
                var info = await _lockoutStore.GetUserInfo(user.Id);
                return info.Attempts;
            }
            catch (RecordNotFoundException)
            {
                return 0;
            }
        }

        /// <summary>
        ///     Whether or not the user can be locked out
        /// </summary>
        public Task<bool> GetLockoutEnabledAsync(RegiAuthUser user)
        {
            return Task.FromResult(true);
        }

        public Task SetLockoutEnabledAsync(RegiAuthUser user, bool enabled)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region Passwords

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

        #endregion

        #region Users

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
            // Delete user foreign values
            await _fieldStore.DeleteValuesForUser(user);
            await _lockoutStore.DeleteInfoForUser(user.Id);
            await _tokenStore.DeleteTokensForUser(user.Id);

            // Delete main record
            await _store.DeleteUser(user.Id);
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

        #endregion

        #region 2FA

        public Task SetTwoFactorEnabledAsync(RegiAuthUser user, bool enabled)
        {
            return Task.CompletedTask;
        }

        public Task<bool> GetTwoFactorEnabledAsync(RegiAuthUser user)
        {
            return Task.FromResult(false);
        }

        #endregion
    }
}