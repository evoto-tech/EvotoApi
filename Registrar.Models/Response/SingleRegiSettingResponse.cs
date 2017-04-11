using System.Runtime.Serialization;

namespace Registrar.Models.Response
{
    [DataContract]
    public class SingleRegiSettingResponse
    {
        public SingleRegiSettingResponse()
        {

        }

        public SingleRegiSettingResponse(RegiSetting setting)
        {
            Name = setting.Name;
            Value = setting.Value;
        }

        [DataMember(Name = "name")]
        public string Name { get; private set; }

        [DataMember(Name = "value")]
        public string Value { get; private set; }
    }
}