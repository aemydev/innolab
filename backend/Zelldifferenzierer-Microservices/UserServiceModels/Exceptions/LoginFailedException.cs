using System;

namespace UserServiceModels.Exceptions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException(string message) : base (message) { }
        public LoginFailedException(string message, Exception inner) : base(message, inner) { }
    }
}
