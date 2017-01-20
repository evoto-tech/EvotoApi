using System;
using Microsoft.AspNet.Identity;

namespace Registrar.Models
{
    public class RegiUser : IUser<string>
    {
        private int _id;

        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string Id
        {
            get { return Convert.ToString(_id); }
            set { _id = Convert.ToInt32(value); }
        }

        public string UserName
        {
            get { return Email; }
            set { Email = value; }
        }
    }
}