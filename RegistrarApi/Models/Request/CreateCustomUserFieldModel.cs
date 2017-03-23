using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Common.Models;

namespace Registrar.Api.Models.Request
{
    [DataContract]
    public class CreateCustomUserFieldModel
    {
        [DataMember(Name = "id")]
        [Required]
        public int Id { get; private set; }

        [DataMember(Name = "name")]
        [Required]
        public string Name { get; private set; }

        [DataMember(Name = "type")]
        [Required]
        [EnumDataType(typeof(EUserFieldType))]
        public EUserFieldType Type { get; set; }

        [DataMember(Name = "required")]
        [Required]
        public bool Required { get; set; }

        [DataMember(Name = "validation")]
        [Required]
        public dynamic Validation { get; set; }
    }
}