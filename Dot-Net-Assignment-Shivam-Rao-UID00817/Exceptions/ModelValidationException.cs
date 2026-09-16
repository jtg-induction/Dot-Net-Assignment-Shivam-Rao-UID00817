using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions
{
    public class ModelValidationException: Exception
    {
        public ModelValidationException(string message) : base(message)
        {
        }
    }
}
