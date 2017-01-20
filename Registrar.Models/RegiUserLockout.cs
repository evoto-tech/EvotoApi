using System;

namespace Registrar.Models
{
    public class RegiUserLockout
    {
        public int UserId { get; set; }
        public int Attempts { get; set; }
        public DateTime LockEnd { get; set; }
    }
}