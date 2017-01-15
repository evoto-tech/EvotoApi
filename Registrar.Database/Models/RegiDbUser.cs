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
        }

        public RegiDbUser(RegiUser model)
        {
            if (model == null)
                throw new ArgumentException(nameof(model));

            Id = (uint) model.Id;
            Email = model.Email;
            PasswordHash = model.PasswordHash;
        }

        public uint Id { get; }
        public string Email { get; }
        public string PasswordHash { get; }

        public RegiUser ToUser()
        {
            return new RegiUser
            {
                Id = (int) Id,
                Email = Email,
                PasswordHash = PasswordHash
            };
        }
    }
}