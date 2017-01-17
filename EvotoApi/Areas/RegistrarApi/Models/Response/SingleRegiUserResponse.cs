using System;
using System.Runtime.Serialization;
using Registrar.Models;

namespace EvotoApi.Areas.RegistrarApi.Models.Response
{
    [Serializable]
    public class SingleRegiUserResponse
    {
        public SingleRegiUserResponse(RegiUser user)
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