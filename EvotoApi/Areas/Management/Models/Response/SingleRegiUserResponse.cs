using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Common.Models;

namespace EvotoApi.Areas.ManagementApi.Models.Response
{
    [DataContract]
    public class SingleRegiUserResponse
    {
        public SingleRegiUserResponse(RegiUser user)
        {
            Id = user.Id;
            Email = user.Email;

            CustomFields = user.CustomFields.ToDictionary(cf => cf.Name, cf => cf.Value);
        }

        [DataMember(Name = "id")]
        public int Id { get; }

        [DataMember(Name = "email")]
        public string Email { get; }

        [DataMember(Name = "customFields")]
        public IDictionary<string, string> CustomFields { get; }

        [DataMember(Name = "emailConfirmed")]
        public bool EmailConfirmed { get; }
    }
}