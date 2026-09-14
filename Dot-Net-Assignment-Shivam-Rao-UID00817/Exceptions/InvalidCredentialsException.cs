using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException(string message) : base(message) 
        { 
        }
    }
}
