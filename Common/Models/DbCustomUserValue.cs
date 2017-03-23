namespace Common.Models
{
    public class DbCustomUserValue
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public CustomUserValue ToModel()
        {
            return new CustomUserValue
            {
                Name = Name,
                Value = Value
            };
        }
    }
}