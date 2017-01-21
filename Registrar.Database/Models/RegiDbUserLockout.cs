using System;
using Registrar.Models;

namespace Registrar.Database.Models
{
    public class RegiDbUserLockout
    {
        public RegiDbUserLockout(dynamic record)
        {
            UserId = record.UserId;
            Attempts = record.Attempts;
            if (record.LockEnd != null)
                LockEnd = record.LockEnd;
        }

        public RegiDbUserLockout(RegiUserLockout model)
        {
            if (model == null)
                throw new ArgumentException(nameof(model));

            UserId = UserId;
            Attempts = model.Attempts;
            LockEnd = model.LockEnd;
        }

        public int UserId { get; }
        public int Attempts { get; }
        public DateTime LockEnd { get; }

        public RegiUserLockout ToUser()
        {
            return new RegiUserLockout
            {
                UserId = UserId,
                Attempts = Attempts,
                LockEnd = LockEnd
            };
        }
    }
}