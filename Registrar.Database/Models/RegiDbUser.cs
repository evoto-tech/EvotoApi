﻿using System;
using System.Collections.Generic;
using System.Linq;
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

            var rowDict = (IDictionary<string, object>) record;
            var builtInTypes = GetType().GetProperties();

            // Ignore the properties we already have in this class (Id, Email, etc.)
            CustomFields =
                rowDict.Where(
                        kv => builtInTypes.All(t => !t.Name.Equals(kv.Key, StringComparison.InvariantCultureIgnoreCase)))
                    .Select(kv => new DbCustomUserValueOut {Name = kv.Key, Value = (string) kv.Value})
                    .ToList();
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

        public IList<DbCustomUserValueOut> CustomFields { get; }

        public RegiUser ToUser()
        {
            return new RegiUser
            {
                Id = Id,
                Email = Email,
                PasswordHash = PasswordHash,
                EmailConfirmed = EmailConfirmed,
                CustomFields = CustomFields.Select(cf => cf.ToModel()).ToList()
            };
        }
    }
}