using Common.Models;

namespace Registrar.Database.Models
{
    public class RegiDbUserCustomField
    {
        public RegiDbUserCustomField(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public CustomUserField ToModel()
        {
            return new CustomUserField
            {
                Name = Name,
                Value = Value
            };
        }
    }
}