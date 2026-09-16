using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Helpers
{
    public class HashingHelper
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public static bool VerifyPassword(string p , string hash)
        {
            return BCrypt.Net.BCrypt.Verify(p , hash);
        }
    }
}
