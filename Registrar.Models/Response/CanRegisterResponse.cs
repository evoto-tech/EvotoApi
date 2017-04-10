using System.Runtime.Serialization;

namespace Registrar.Models.Response
{
    [DataContract]
    public class CanRegisterResponse
    {
        public CanRegisterResponse(bool enabled)
        {
            Enabled = enabled;
        }

        [DataMember(Name = "enabled")]
        public bool Enabled { get; private set; }
    }
}