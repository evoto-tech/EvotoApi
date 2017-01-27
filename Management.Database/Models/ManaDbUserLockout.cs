using System;
using Management.Models;

namespace Management.Database.Models
{
    public class ManaDbUserLockout
    {
        public ManaDbUserLockout(dynamic record)
        {
            UserId = record.UserId;
            Attempts = record.Attempts;
            if (record.LockEnd != null)
                LockEnd = record.LockEnd;
        }

        public ManaDbUserLockout(ManaUserLockout model)
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

        public ManaUserLockout ToUser()
        {
            return new ManaUserLockout
            {
                UserId = UserId,
                Attempts = Attempts,
                LockEnd = LockEnd
            };
        }
    }
}