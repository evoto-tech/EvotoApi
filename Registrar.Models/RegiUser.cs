using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Registrar.Models;

namespace Registrar.Database.Models
{
    public class RegiUser : IUser<int>
    {
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public bool EmailConfirmed { get; set; }

        public int Id { get; set; }

        public IList<CustomUserValue> CustomFields { get; set; }

        public string UserName
        {
            get { return Email; }
            set { Email = value; }
        }
    }
}