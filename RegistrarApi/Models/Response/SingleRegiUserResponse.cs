﻿using System.Runtime.Serialization;
using Common.Models;

namespace Registrar.Api.Models.Response
{
    [DataContract]
    public class SingleRegiUserResponse
    {
        public SingleRegiUserResponse(RegiUser user)
        {
            Id = user.Id;
            Email = user.Email;
            EmailConfirmed = user.EmailConfirmed;
        }

        [DataMember(Name = "id")]
        public int Id { get; }

        [DataMember(Name = "email")]
        public string Email { get; }

        [DataMember(Name = "emailConfirmed")]
        public bool EmailConfirmed { get; }
    }
}