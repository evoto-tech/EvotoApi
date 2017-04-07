using Common.Models;
using Registrar.Models;

namespace Registrar.Database.Models
{
    public class DbCustomUserValueIn
    {
        public int UserId { get; set; }

        public int CustomFieldId { get; set; }

        public string Value { get; set; }

        public DbCustomUserValueIn(RegiUser user, CustomUserValue value)
        {
            UserId = user.Id;
            CustomFieldId = value.FieldId;
            Value = value.Value;
        }
    }
}