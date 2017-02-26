using System.Runtime.Serialization;
using Management.Models;

namespace Registrar.Api.Models.Response
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