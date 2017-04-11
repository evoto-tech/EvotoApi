using System;

namespace Registrar.Models
{
    public class RefreshToken
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Ticket { get; set; }
        public DateTime Issued { get; set; }
        public DateTime Expires { get; set; }

        public bool IsExpired => Expires <= DateTime.Now;
    }
}