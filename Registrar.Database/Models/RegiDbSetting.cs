using Registrar.Models;

namespace Registrar.Database.Models
{
    public class RegiDbSetting
    {
        public RegiDbSetting(dynamic record)
        {
            Name = record.Name;
            Value = record.Value;
        }

        public string Name { get; }

        public string Value { get; }
    }
}