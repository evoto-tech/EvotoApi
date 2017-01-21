using System;

namespace Common.Models
{
    public class DbRefreshToken
    {
        public DbRefreshToken(RefreshToken model)
        {
            UserId = model.UserId;
            Token = model.Token;
            Ticket = model.Ticket;
            Issued = model.Issued;
            Expires = model.Expires;
        }

        public DbRefreshToken(dynamic record)
        {
            UserId = record.UserId;
            Token = record.Token;
            Ticket = record.Ticket;
            Issued = record.Issued;
            Expires = record.Expires;
        }

        public int UserId { get; set; }
        public string Token { get; set; }
        public string Ticket { get; set; }
        public DateTime Issued { get; set; }
        public DateTime Expires { get; set; }

        public RefreshToken ToModel()
        {
            return new RefreshToken
            {
                UserId = UserId,
                Token = Token,
                Ticket = Ticket,
                Issued = Issued,
                Expires = Expires
            };
        }
    }
}