namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Constants
{
    public static class REGEX
    {
        public const string PASSWORD_REGEX = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$";

        public const string EMAIL_REGEX = @"^\s*[^@\s]+@[^@\s]+\.[^@\s]+\s*$";
    }
}
