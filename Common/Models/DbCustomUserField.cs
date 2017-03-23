using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Common.Models
{
    public class DbCustomUserField
    {
        public DbCustomUserField(dynamic row)
        {
            Id = row.Id;
            Name = row.Name;
            Validation = row.Validation;
            Type = row.Type;
            Required = row.Required;
        }

        public DbCustomUserField(CustomUserField<dynamic> model)
        {
            Id = model.Id;
            Name = model.Name;
            Type = model.Type.ToString();
            Required = model.Required;

            Validation = JsonConvert.SerializeObject(model.GetValidationProperties());
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Validation { get; set; }

        public string Type { get; set; }

        public bool Required { get; set; }

        public CustomUserField ToModel()
        {
            EUserFieldType type;
            if (!Enum.TryParse(Type, out type))
                return null;

            var fieldType = typeof(CustomUserField);
            var fieldName = fieldType.Namespace + "." + fieldType.Name + "_" + type;
            var fieldClass = typeof(CustomUserField).Assembly.GetType(fieldName);
            var constructor = fieldClass.GetConstructor(new Type[] {});
            if (constructor == null)
                return null;

            var field = (CustomUserField) constructor.Invoke(new object[] {});

            field.Id = Id;
            field.Name = Name;
            field.Type = type;
            field.Required = Required;

            var props = JsonConvert.DeserializeObject<dynamic>(Validation);
            field.SetValidationProperties(props);

            return field;
        }
    }
}