using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace Registrar.Models
{
    public sealed class CustomUserValidation : DynamicObject
    {
        public delegate object Getter(dynamic target);

        private readonly dynamic _val;

        public CustomUserValidation(dynamic val)
        {
            _val = val;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            JToken value;
            var key = char.ToLower(binder.Name[0]) + binder.Name.Substring(1);

            result = _val.TryGetValue(key, out value) ? value?.ToString() : null;
            return true;
        }
    }
}