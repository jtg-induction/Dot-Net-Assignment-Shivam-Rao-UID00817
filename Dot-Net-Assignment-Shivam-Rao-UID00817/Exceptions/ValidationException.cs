using System;
using System.Collections.Generic;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions
{
    public class ValidationException : Exception
    {
        public List<String> ValidationMessages { get; set; }
        
        public ValidationException(string message) : base(message)
        {
            ValidationMessages.Add(message);
        }

        public ValidationException(List<String> validationMessages)
        {
            this.ValidationMessages = validationMessages;
        }
    }
}
