using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Microsoft.Owin;
using System;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Helpers
{
    public class CookieHelper
    {
        public static CookieOptions GetCookieOptions(string path, int ExpiresInDays)
        {
            return new CookieOptions
            {
                HttpOnly = true ,
                Secure = true ,
                SameSite = Microsoft.Owin.SameSiteMode.Lax ,
                Expires = DateTime.UtcNow.AddDays(ExpiresInDays) ,
                Path = path
            };
        }
    }
}
