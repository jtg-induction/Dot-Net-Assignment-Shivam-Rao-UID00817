using System;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException()
        {

        }
    }
}
