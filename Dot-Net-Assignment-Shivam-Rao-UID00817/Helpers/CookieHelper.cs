using Microsoft.Owin;
using System;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Helpers
{
    public class CookieHelper
    {
        /// <summary>
        /// Creates and returns cookie options with the specified path and expiration period.
        /// </summary>
        /// <param name="path">The path where the cookie is valid.</param>
        /// <param name="ExpiresInDays">The number of days until the cookie expires.</param>
        /// <returns>Configured cookie options.</returns>
        public static CookieOptions GetCookieOptions(string path, int ExpiresInDays)
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = Microsoft.Owin.SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(ExpiresInDays),
                Path = path
            };
        }
    }
}
