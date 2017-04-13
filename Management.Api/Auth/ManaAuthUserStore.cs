using System;
using System.Threading.Tasks;
using Common.Exceptions;
using Management.Database.Interfaces;
using Management.Models;
using Microsoft.AspNet.Identity;

namespace EvotoApi.Auth
{
    public class ManaAuthUserStore : IUserPasswordStore<ManaAuthUser, int>,
        IUserEmailStore<ManaAuthUser, int>, IUserLockoutStore<ManaAuthUser, int>, IUserTwoFactorStore<ManaAuthUser, int>
    {
        private readonly IManaUserLockoutStore _lockoutStore;
        private readonly IManaUserStore _store;

        public ManaAuthUserStore(IManaUserStore store, IManaUserLockoutStore lockoutStore)
        {
            _store = store;
            _lockoutStore = lockoutStore;
        }

        public void Dispose()
        {
        }

        #region Emails

        public async Task SetEmailAsync(ManaAuthUser user, string email)
        {
            user.Email = email;
            await _store.UpdateUser(user);
        }

        public Task<string> GetEmailAsync(ManaAuthUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ManaAuthUser user)
        {
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(ManaAuthUser user, bool confirmed)
        {
            return Task.CompletedTask;
        }

        public async Task<ManaAuthUser> FindByEmailAsync(string email)
        {
            return await FindByNameAsync(email);
        }

        #endregion

        #region Lockout

        public async Task<DateTimeOffset> GetLockoutEndDateAsync(ManaAuthUser user)
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

        public async Task SetLockoutEndDateAsync(ManaAuthUser user, DateTimeOffset lockoutEnd)
        {
            var info = new ManaUserLockout {LockEnd = lockoutEnd.DateTime.ToLocalTime(), UserId = user.Id};
            try
            {
                await _lockoutStore.UpdateUserTime(info);
            }
            catch (RecordNotFoundException)
            {
                await _lockoutStore.InsertUserTime(info);
            }
        }

        public async Task<int> IncrementAccessFailedCountAsync(ManaAuthUser user)
        {
            ManaUserLockout info;
            try
            {
                info = await _lockoutStore.GetUserInfo(user.Id);
                info.Attempts++;
                await _lockoutStore.UpdateUserAttempts(info);
            }
            catch (RecordNotFoundException)
            {
                info = new ManaUserLockout
                {
                    UserId = user.Id,
                    Attempts = 1
                };
                await _lockoutStore.InsertUserAttempts(info);
            }

            return info.Attempts;
        }

        public async Task ResetAccessFailedCountAsync(ManaAuthUser user)
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

        public async Task<int> GetAccessFailedCountAsync(ManaAuthUser user)
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
        public Task<bool> GetLockoutEnabledAsync(ManaAuthUser user)
        {
            return Task.FromResult(true);
        }

        public Task SetLockoutEnabledAsync(ManaAuthUser user, bool enabled)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region Passwords

        public async Task SetPasswordHashAsync(ManaAuthUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            await UpdateAsync(user);
        }

        public async Task<string> GetPasswordHashAsync(ManaAuthUser user)
        {
            var model = await FindByIdAsync(user.Id);
            return model?.PasswordHash;
        }

        public Task<bool> HasPasswordAsync(ManaAuthUser user)
        {
            return Task.FromResult(true);
        }

        #endregion

        #region Users

        public async Task CreateAsync(ManaAuthUser user)
        {
            await _store.CreateUser(user);
        }

        public async Task UpdateAsync(ManaAuthUser user)
        {
            await _store.UpdateUser(user);
        }

        public async Task DeleteAsync(ManaAuthUser user)
        {
            await _store.DeleteUser(Convert.ToInt32(user.Id));
        }

        public async Task<ManaAuthUser> FindByIdAsync(int userId)
        {
            try
            {
                var user = await _store.GetUserById(userId);
                return new ManaAuthUser(user);
            }
            catch (RecordNotFoundException)
            {
                return null;
            }
        }

        public async Task<ManaAuthUser> FindByNameAsync(string userName)
        {
            try
            {
                var user = await _store.GetUserByEmail(userName);
                return new ManaAuthUser(user);
            }
            catch (RecordNotFoundException)
            {
                return null;
            }
        }

        #endregion

        #region 2FA

        public Task SetTwoFactorEnabledAsync(ManaAuthUser user, bool enabled)
        {
            return Task.CompletedTask;
        }

        public Task<bool> GetTwoFactorEnabledAsync(ManaAuthUser user)
        {
            return Task.FromResult(false);
        }

        #endregion
    }
}