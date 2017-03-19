using Microsoft.AspNet.Identity;

namespace Registrar.Models
{
    public class RegiUser : IUser<int>
    {
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public int Id { get; set; }

        public bool EmailConfirmed { get; set; }

        public string UserName
        {
            get { return Email; }
            set { Email = value; }
        }
    }
}