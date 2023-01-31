using System;

namespace UserServiceModels.Exceptions
{
    public class ContainsCharactersException : Exception
    {
        private new const string Message = "Element does not contain numbers only!";

        public ContainsCharactersException() : base(Message)
        {

        }
        public ContainsCharactersException(string message) : base(Message)
        {

        }

        public ContainsCharactersException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
