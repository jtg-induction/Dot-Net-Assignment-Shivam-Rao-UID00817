using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions
{
    public class UserAlreadyExistsException: Exception
    {
        public UserAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
