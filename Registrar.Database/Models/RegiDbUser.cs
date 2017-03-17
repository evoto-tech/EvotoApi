using System;
using Registrar.Models;

namespace Registrar.Database.Models
{
    public class RegiDbUser
    {
        public RegiDbUser(dynamic record)
        {
            Id = record.Id;
            Email = record.Email;
            PasswordHash = record.PasswordHash;
            EmailConfirmed = record.EmailConfirmed;
        }

        public RegiDbUser(RegiUser model)
        {
            if (model == null)
                throw new ArgumentException(nameof(model));

            Id = model.Id;
            Email = model.Email;
            PasswordHash = model.PasswordHash;
            EmailConfirmed = model.EmailConfirmed;
        }

        public int Id { get; }

        public string Email { get; }

        public string PasswordHash { get; }

        public bool EmailConfirmed { get; }

        public RegiUser ToUser()
        {
            return new RegiUser
            {
                Id = Id,
                Email = Email,
                PasswordHash = PasswordHash,
                EmailConfirmed = EmailConfirmed
            };
        }
    }
}