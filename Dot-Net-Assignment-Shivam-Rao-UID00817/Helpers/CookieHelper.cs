using Microsoft.Owin;
using System;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Helpers
{
    public class CookieHelper
    {
        public static CookieOptions CreateCookieOptions(string path, int ExpiresInDays)
        {
            return new CookieOptions
            {
                HttpOnly = true ,
                Secure = true ,
                SameSite = SameSiteMode.Lax ,
                Expires = DateTime.UtcNow.AddDays(ExpiresInDays) ,
                Path = path
            };
        }
    }
}
