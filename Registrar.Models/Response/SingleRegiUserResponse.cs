using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Registrar.Models.Response
{
    [DataContract]
    public class SingleRegiUserResponse
    {
        public SingleRegiUserResponse(RegiUser user)
        {
            if (user == null)
                return;

            Id = user.Id;
            Email = user.Email;
            EmailConfirmed = user.EmailConfirmed;

            CustomFields = user.CustomFields.ToDictionary(cf => cf.Name, cf => cf.Value);
        }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "customFields")]
        public IDictionary<string, string> CustomFields { get; set; }

        [DataMember(Name = "emailConfirmed")]
        public bool EmailConfirmed { get; set; }
    }
}