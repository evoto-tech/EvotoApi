using System;
using Management.Models;

namespace Management.Database.Models
{
    public class ManaDbUser
    {
        public ManaDbUser(dynamic record)
        {
            Id = record.Id;
            Email = record.Email;
            PasswordHash = record.PasswordHash;
        }

        public ManaDbUser(ManaUser model)
        {
            if (model == null)
                throw new ArgumentException(nameof(model));

            Id = model.Id;
            Email = model.Email;
            PasswordHash = model.PasswordHash;
        }

        public int Id { get; }
        public string Email { get; }
        public string PasswordHash { get; }

        public ManaUser ToUser()
        {
            return new ManaUser
            {
                Id = Id,
                Email = Email,
                PasswordHash = PasswordHash
            };
        }
    }
}