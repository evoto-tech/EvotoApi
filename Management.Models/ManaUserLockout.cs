using System;

namespace Management.Models
{
    public class ManaUserLockout
    {
        public int UserId { get; set; }
        public int Attempts { get; set; }
        public DateTime LockEnd { get; set; }
    }
}