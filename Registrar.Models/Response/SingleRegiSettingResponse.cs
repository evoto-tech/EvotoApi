using System.Runtime.Serialization;
using Registrar.Models.Request;

namespace Registrar.Models.Response
{
    [DataContract]
    public class SingleRegiSettingResponse
    {
        public SingleRegiSettingResponse () {}

        public SingleRegiSettingResponse(UpdateRegiSetting setting)
        {
            if (setting == null)
                return;

            Name = setting.Name;
            Value = setting.Value;
        }

        public SingleRegiSettingResponse(dynamic record)
        {
            Name = record.Name;
            Value = record.Value;
        }

        [DataMember(Name = "name")]
        public string Name { get; private set; }

        [DataMember(Name = "value")]
        public string Value { get; private set; }
    }
}