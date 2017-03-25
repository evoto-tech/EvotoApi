using System.Collections.Generic;
using System.Runtime.Serialization;
using Common.Models;

namespace Registrar.Api.Models.Response
{
    [DataContract]
    public class SingleCustomUserFieldResponse
    {
        public SingleCustomUserFieldResponse(CustomUserField field)
        {
            Id = field.Id;
            Name = field.Name;
            Type = field.Type.ToString();
            Required = field.Required;

            var props = field.GetValidationProperties();
            var propTypes = props.GetType().GetProperties();
            Validation = new Dictionary<string, string>();
            foreach (var prop in propTypes)
            {
                var name = char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1);
                Validation.Add(name, (string) (prop.GetValue(props) ?? "null"));
            }
        }

        [DataMember(Name = "id")]
        public int Id { get; }

        [DataMember(Name = "name")]
        public string Name { get; }

        [DataMember(Name = "type")]
        public string Type { get; }

        [DataMember(Name = "required")]
        public bool Required { get; }

        [DataMember(Name = "validation")]
        public IDictionary<string, string> Validation { get; }
    }
}