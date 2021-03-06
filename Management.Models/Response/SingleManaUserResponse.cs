using System.Runtime.Serialization;

namespace Management.Models.Response
{
    [DataContract]
    public class SingleManaUserResponse
    {
        public SingleManaUserResponse(ManaUser user)
        {
            Id = user.Id;
            Email = user.Email;
        }

        [DataMember(Name = "id")]
        public int Id { get; }

        [DataMember(Name = "email")]
        public string Email { get; }
    }
}