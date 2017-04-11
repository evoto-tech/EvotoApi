using Registrar.Models;

namespace Registrar.Database.Models
{
    public class DbCustomUserValueIn
    {
        public DbCustomUserValueIn(RegiUser user, CustomUserValue value)
        {
            UserId = user.Id;
            CustomFieldId = value.FieldId;
            Value = value.Value;
        }

        public int UserId { get; set; }

        public int CustomFieldId { get; set; }

        public string Value { get; set; }
    }
}