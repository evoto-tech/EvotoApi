using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Common.Models;

namespace Registrar.Models.Request
{
    [DataContract]
    public class CreateCustomUserFieldModel
    {
        [DataMember(Name = "id")]
        public int Id { get; private set; }

        [DataMember(Name = "name")]
        [Required]
        public string Name { get; private set; }

        [DataMember(Name = "type")]
        [Required]
        [EnumDataType(typeof(EUserFieldType))]
        public EUserFieldType Type { get; private set; }

        [DataMember(Name = "required")]
        [Required]
        public bool Required { get; private set; }

        [DataMember(Name = "validation")]
        [Required]
        public dynamic Validation { get; private set; }
    }
}