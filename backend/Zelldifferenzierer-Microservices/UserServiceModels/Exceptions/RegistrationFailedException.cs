using System;

namespace UserServiceModels.Exceptions
{
    public class RegistrationFailedException : Exception
    {
        public RegistrationFailedException(string message, Exception inner) : base(message, inner) { }
        public RegistrationFailedException(string message) : base(message) { }
    }
}
