namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Constants
{
    public static class ErrorMessages
    {
        public const string USER_ALREADY_EXISTS = "User Already Exists.";

        public const string INVALID_CREDENTIALS = "Invalid Credentials.";

        public const string INVALID_REFRESH_TOKEN = "Refresh Token is Invalid.";

        public const string INVALID_EMAIL_FORMAT = "Email must be a valid format (e.g., user@example.com).";

        public const string INVALID_PASSWORD_FORMAT = "Password must be between 8 - 15 characters in length and must contain atleast a digit, an uppercase letter, a lowercase letter and a special character.";

        public const string MODEL_WAS_NULL = "No model was supplied.";

        public const string DUPLICATE_EMAIL = "Email already exists.";

        public const string DUPLICATE_PHONE_NUMBER = "Phone Number already exists.";

        public const string NO_AUTH_TOKEN = "Authorization token is required";

        public const string RESTAURANT_ALREADY_EXISTS = "Restaurant with same name aleady exists";

        public const string USER_DOESNOT_EXIST = "User with the provided Email does not exist";

        public const string RESTAURANT_DOESNOT_EXIST = "Restaurant with the provided name does not exist";

        public const string USER_ALREADY_ONBOARDED = "The user with the email is already onboarded to the restaurant.";

        public const string NO_VALID_EMAILS = "Valid owner emails were not provided.";

        public const string USER_NOT_ACTIVE = "Account with given email is not active";

        public const string CANNOT_ASSIGN_OWNER_TO_RESTAURANT_THAT_IS_NOT_ACTIVE = "Cannot Assign owners to restaurant that is not active.";
    }
}
