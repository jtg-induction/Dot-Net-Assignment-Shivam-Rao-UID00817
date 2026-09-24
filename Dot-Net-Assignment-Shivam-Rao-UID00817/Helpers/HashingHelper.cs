namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Helpers
{
    public class HashingHelper
    {
        /// <summary>
        /// Hashes a password using BCrypt.
        /// </summary>
        /// <param name="password">The plain-text password to hash.</param>
        /// <returns>The hashed password.</returns>
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Verifies a plain-text password against a BCrypt hash.
        /// </summary>
        /// <param name="p">The plain-text password to verify.</param>
        /// <param name="hash">The stored password hash.</param>
        /// <returns>True if the password matches the hash; otherwise, false.</returns>
        public static bool VerifyPassword(string p , string hash)
        {
            return BCrypt.Net.BCrypt.Verify(p , hash);
        }
    }
}
