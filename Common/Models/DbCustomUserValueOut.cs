namespace Common.Models
{
    public class DbCustomUserValueOut
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