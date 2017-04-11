using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Registrar.Models.Response
{
    [DataContract]
    public class SingleCustomUserFieldResponse
    {
        public SingleCustomUserFieldResponse()
        {
        }

        public SingleCustomUserFieldResponse(CustomUserField field)
        {
            Id = field.Id;
            Name = field.Name;
            Type = field.Type.ToString();
            Required = field.Required;

            var props = field.GetValidationProperties();
            var propTypes = props.GetType().GetProperties();
            Validation = new Dictionary<string, object>();
            foreach (var prop in propTypes)
            {
                var name = char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1);
                Validation.Add(name, prop.GetValue(props));
            }
        }

        [DataMember(Name = "id")]
        public int Id { get; private set; }

        [DataMember(Name = "name")]
        public string Name { get; private set; }

        [DataMember(Name = "type")]
        public string Type { get; private set; }

        [DataMember(Name = "required")]
        public bool Required { get; private set; }

        [DataMember(Name = "validation")]
        public IDictionary<string, object> Validation { get; private set; }
    }
}