using System;

namespace Management.Models.Exceptions
{
    public class RegistrarConnectionException : Exception
    {
        public RegistrarConnectionException(string msg) : base(msg)
        {
        }
    }
}