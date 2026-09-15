using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817
{
    public class Constants
    {
        public const string PHONE_NUMBER_REGEX = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$";

        public const string EMAIL_REGEX = @"^\s*[^@\s]+@[^@\s]+\.[^@\s]+\s*$";

        public const string USER_ALREADY_EXISTS = "Email / Phone Number already exists.";

        public const string INVALID_CREDENTIALS = "Email / Password is incorrect";
    }
}
