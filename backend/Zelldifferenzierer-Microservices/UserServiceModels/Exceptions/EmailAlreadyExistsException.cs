using System;
using System.Collections.Generic;
using System.Text;

namespace UserServiceModels.Exceptions
{
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException(string message) : base(message) { }
        public EmailAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
    }
}
