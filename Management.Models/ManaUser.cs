namespace Management.Models
{
    public class ManaUser
    {
        public uint Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}