using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common.Exceptions;
using Common.Models;
using Microsoft.AspNet.Identity;
using Registrar.Database.Interfaces;

namespace Registrar.Api.Auth
{
    public class RegiTokenProvider : IUserTokenProvider<RegiAuthUser, int>
    {
        private readonly IRegiUserTokenStore _store;

        public RegiTokenProvider()
        {
            _store = (IRegiUserTokenStore) DependencyResolver.Current.GetService(typeof(IRegiUserTokenStore));
        }

        public async Task<string> GenerateAsync(string purpose, UserManager<RegiAuthUser, int> manager,
            RegiAuthUser user)
        {
            var token = CreateToken(purpose, user.Id);

            try
            {
                // Will throw exception if doesn't exist, and call insert instead of update
                await _store.GetRefreshTokenForUser(purpose, user.Id);

                // TODO: Refactor to better encompass logic if updating fails
                
                await _store.UpdateUserToken(token);
            }
            catch (RecordNotFoundException)
            {
                await _store.CreateUserToken(token);
            }

            return token.Token;
        }

        public async Task<bool> ValidateAsync(string purpose, string providedToken, UserManager<RegiAuthUser, int> manager,
            RegiAuthUser user)
        {
            try
            {
                var token = await _store.GetRefreshTokenForUser(purpose, user.Id);

                if (!token.Expired && providedToken == token.Token)
                {
                    await _store.DeleteUserToken(token);
                    return true;
                }
            }
            catch (RecordNotFoundException)
            {
            }
            return false;
        }

        public Task NotifyAsync(string token, UserManager<RegiAuthUser, int> manager, RegiAuthUser user)
        {
            return Task.CompletedTask;
        }

        public Task<bool> IsValidProviderForUserAsync(UserManager<RegiAuthUser, int> manager, RegiAuthUser user)
        {
            if (manager == null)
                throw new ArgumentNullException();
            return Task.FromResult(manager.SupportsUserPassword);
        }

        private static UserToken CreateToken(string purpose, int userId)
        {
            return new UserToken
            {
                Expires = DateTime.Now.AddHours(Startup.UserTokenTime),
                UserId = userId,
                Purpose = purpose,
                Token = Guid.NewGuid().ToString("n").Substring(0, 10)
            };
        }
    }
}