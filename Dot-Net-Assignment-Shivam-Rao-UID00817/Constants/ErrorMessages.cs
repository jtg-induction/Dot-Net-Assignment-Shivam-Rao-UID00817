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

        public const string USER_DOES_NOT_EXIST = "User with the provided Email does not exist";

        public const string RESTAURANT_DOES_NOT_EXIST = "Restaurant does not exist";

        public const string USER_ALREADY_ONBOARDED = "The user with the email is already onboarded to the restaurant.";

        public const string NO_VALID_EMAILS = "Valid owner emails were not provided.";

        public const string USER_NOT_ACTIVE = "Account with given email is not active";

        public const string CANNOT_ASSIGN_OWNER_TO_RESTAURANT_THAT_IS_NOT_ACTIVE = "Cannot Assign owners to restaurant that is not active.";

        public const string ADDRESS_DOES_NOT_EXIST = "Address doesn't exist";

        public const string ATLEAST_ONE_ITEM_REQUIRED = "Atleast one item is required to place order.";

        public const string INVALID_ITEMS = "One or more Items were not valid";

        public const string REQUIRED_QUANTITY_NOT_AVAILABLE = "Required quantity not available";

        public const string INSUFFICIENT_WALLET_BALANCE = "Insufficient Wallet Balance";

        public const string INVALID_ITEM_QUANTITY = "Item quantity invalid.";

        public const string ORDER_DOES_NOT_EXIST = "Order with given order id does not exist";

        public const string CANNOT_CANCEL_ORDER_AFTER_IT_HAS_BEEN_ACCEPTED = "Cannot cancel an order after it has been accepted.";

        public const string INVALID_OPERATION = "Operation is invalid";
    }
}
