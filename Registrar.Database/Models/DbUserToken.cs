using System;
using Registrar.Models;

namespace Common.Models
{
    public class DbUserToken
    {
        public DbUserToken(dynamic record)
        {
            UserId = record.UserId;
            Purpose = record.Purpose;
            Token = record.Token;
            Expires = record.Expires;
            Created = record.Created;
        }

        public DbUserToken(UserToken model)
        {
            if (model == null)
                throw new ArgumentException(nameof(model));

            UserId = model.UserId;
            Purpose = model.Purpose;
            Token = model.Token;
            Expires = model.Expires;
            Created = model.Created;
        }

        public string Purpose { get; }
        public int UserId { get; }
        public string Token { get; }
        public DateTime Created { get; }
        public DateTime Expires { get; }

        public UserToken ToToken()
        {
            return new UserToken
            {
                UserId = UserId,
                Purpose = Purpose,
                Token = Token,
                Expires = Expires,
                Created = Created
            };
        }
    }
}