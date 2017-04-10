using Registrar.Models.Exceptions;

namespace Registrar.Models
{
    public class RegiSetting
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public bool GetBoolValue()
        {
            bool b;
            if (bool.TryParse(Value, out b))
                return b;

            throw new InvalidSettingException("bool", Name, Value);
        }

        public int GetIntValue()
        {
            int i;
            if (int.TryParse(Value, out i))
                return i;

            throw new InvalidSettingException("int", Name, Value);
        }
    }
}