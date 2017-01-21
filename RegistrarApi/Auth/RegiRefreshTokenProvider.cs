using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common.Exceptions;
using Common.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Infrastructure;
using Registrar.Database.Interfaces;

namespace Registrar.Api.Auth
{
    public class RegiRefreshTokenProvider : AuthenticationTokenProvider
    {
        private readonly IRegiRefreshTokenStore _store;

        public RegiRefreshTokenProvider()
        {
            _store = (IRegiRefreshTokenStore) DependencyResolver.Current.GetService(typeof(IRegiRefreshTokenStore));
        }

        public override async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var userId = context.Ticket.Identity.GetUserId<int>();
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(Startup.RefreshTokenTime));

            var refreshToken = Guid.NewGuid().ToString("n");
            var storeToken = HashToken(refreshToken);

            context.Ticket.Properties.IssuedUtc = DateTime.Now;
            context.Ticket.Properties.ExpiresUtc = expires;

            var token = new RefreshToken
            {
                UserId = userId,
                Token = storeToken,
                Ticket = context.SerializeTicket(),
                Issued = DateTime.Now,
                Expires = expires
            };

            try
            {
                // Will throw exception if doesn't exist, and call insert instead of update
                await _store.GetRefreshTokenForUser(userId);
                await _store.UpdateRefreshToken(token);
            }
            catch (RecordNotFoundException)
            {
                await _store.CreateRefreshToken(token);
            }

            context.SetToken(refreshToken);
        }

        public override async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var hashedTokenId = HashToken(context.Token);

            try
            {
                var refreshToken = await _store.GetRefreshToken(hashedTokenId);
                context.DeserializeTicket(refreshToken.Ticket);
                await _store.DeleteRefreshToken(hashedTokenId);
            }
            catch (RecordNotFoundException)
            {
            }
        }

        private static string HashToken(string token)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            var byteValue = Encoding.UTF8.GetBytes(token);

            var byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }
    }
}