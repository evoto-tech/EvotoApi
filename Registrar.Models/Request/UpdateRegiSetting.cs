using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Registrar.Models.Request
{
    [DataContract]
    public class UpdateRegiSetting
    {
        [DataMember(Name="name")]
        [Required]
        public string Name { get; private set; }

        [DataMember(Name="Value")]
        [Required]
        public string Value { get; private set; }
    }
}