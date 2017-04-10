using System;

namespace Registrar.Models.Exceptions
{
    public class InvalidSettingException : Exception
    {
        public InvalidSettingException(string type, string name, string value)
            : base($"Invalid {type} value for setting {name}: {value}")
        {
        }
    }
}