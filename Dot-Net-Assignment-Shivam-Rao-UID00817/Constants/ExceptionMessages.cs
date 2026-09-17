using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Constants
{
    public static class ExceptionMessages
    {
        public const string USER_ALREADY_EXISTS = "Both Email and Phone Number already exists.";

        public const string INVALID_CREDENTIALS = "Invalid Credentials.";

        public const string INVALID_REFRESH_TOKEN = "Refresh Token is Invalid.";

        public const string INVALID_EMAIL_FORMAT = "Email must be a valid format (e.g., user@example.com).";

        public const string INVALID_PASSWORD_FORMAT = "Password must be between 8 - 15 characters in length and must contain atleast a digit, an uppercase letter, a lowercase letter and a special character.";

        public const string MODEL_WAS_NULL = "No model was supplied.";

        public const string DUPLICATE_EMAIL = "Email already exists.";

        public const string DUPLICATE_PHONE_NUMBER = "Phone Number already exists.";
    }
}
