namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Constants
{
    public static class ErrorMessages
    {
        public const string USER_ALREADY_EXISTS = "User Already Exists.";

        public const string INVALID_CREDENTIALS = "Invalid Credentials.";

        public const string INVALID_REFRESH_TOKEN = "Refresh Token is Invalid.";

        public const string INVALID_EMAIL_FORMAT = "Email must be a valid format (e.g., user@example.com).";

        public static string INVALID_PASSWORD_FORMAT = $"Password must be between {NumberConstants.MIN_PASSWORD_LENGTH} - {NumberConstants.MAX_PASSWORD_LENGTH} characters in length and must contain atleast a digit, an uppercase letter, a lowercase letter and a special character.";

        public const string MODEL_WAS_NULL = "No model was supplied.";

        public const string DUPLICATE_EMAIL = "Email already exists.";

        public const string DUPLICATE_PHONE_NUMBER = "Phone Number already exists.";

        public const string NO_AUTH_TOKEN = "Authorization token is required";
    }
}
