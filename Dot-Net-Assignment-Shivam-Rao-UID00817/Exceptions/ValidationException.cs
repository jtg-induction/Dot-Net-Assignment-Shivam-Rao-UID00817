using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message): base(message)
        {
        }

        public ValidationException()
        {

        }
    }
}
