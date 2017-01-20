using System;
using System.Runtime.Serialization;
using Registrar.Models;

namespace Registrar.Api.Models.Response
{
    [DataContract]
    public class SingleRegiUserResponse
    {
        public SingleRegiUserResponse(RegiUser user)
        {
            Id = Convert.ToInt32(user.Id);
            Email = user.Email;
        }

        [DataMember(Name = "id")]
        public int Id { get; }

        [DataMember(Name = "email")]
        public string Email { get; }
    }
}