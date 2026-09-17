using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services;
using Microsoft.Owin;
using System;
using System.Net.Http;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Helpers
{
    public class CookieHelper
    {
        public static void CreateHttpOnlySecureCookie(string path , AuthService.TokenResult tokenResult , HttpRequestMessage Request)
        {
            IOwinContext owinContext = Request.GetOwinContext();

            owinContext.Response.Cookies.Append("refresh_token" , tokenResult.RefreshToken , new CookieOptions
            {
                HttpOnly = true ,
                Secure = true ,
                SameSite = Microsoft.Owin.SameSiteMode.Lax ,
                Expires = DateTime.UtcNow.AddDays(NumberConstants.REFRESH_TOKEN_EXPIRES_IN_DAYS) ,
                Path = path
            });
        }
    }
}
