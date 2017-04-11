using Microsoft.AspNet.Identity;

namespace Management.Models
{
    public class ManaUser : IUser<int>
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int Id { get; set; }

        public string UserName
        {
            get { return Email; }
            set { Email = value; }
        }
    }
}