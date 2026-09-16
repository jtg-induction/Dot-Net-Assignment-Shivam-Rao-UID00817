using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;
using System.Web.UI;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net;
using System.Web.Http.Description;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using Microsoft.Owin.Security.Provider;
using Microsoft.Owin;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers
{       
    [RoutePrefix("api/auth")]
    public class AuthController: ApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost, Route("register")]
        public async Task<IHttpActionResult> Register([FromBody] RegisterDto model)
        {
            await _authService.RegisterAsync(model);

            return base.Ok(new MessageResponseDto { Message = "Registration successful!" });
        }

        [HttpPost, Route("login")]
        public async Task<IHttpActionResult> Login([FromBody] LoginRequestDto model)
        {
            var tokenResult = await _authService.LoginAsync(model);

            IOwinContext owinContext = Request.GetOwinContext();

            owinContext.Response.Cookies.Append("refresh_token" , tokenResult.RefreshToken, new CookieOptions
            {
                HttpOnly = true ,
                Secure = true ,
                SameSite = Microsoft.Owin.SameSiteMode.Lax ,
                Expires = DateTime.UtcNow.AddDays(NUMBER_CONSTANTS.REFRESH_TOKEN_EXPIRES_IN_DAYS) ,
                Path = "/api/auth/refresh"
            });

            return base.Ok(new LoginResponseDto
            {
                AccessToken = tokenResult.AccessToken ,
                ExpiresIn = NUMBER_CONSTANTS.JWT_EXPIRES_IN_SECONDS
            });
        }

        [HttpPost, Route("refresh")]
        public async Task<IHttpActionResult> Refresh()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["refresh_token"];

            if (cookie == null) return Unauthorized();

            var tokenResult = await _authService.RotateTokenAsync(HttpUtility.UrlDecode(cookie.Value));

            if (tokenResult == null) throw new Exceptions.ValidationException(EXCEPTION_MESSAGES.INVALID_REFRESH_TOKEN);

            IOwinContext owinContext = Request.GetOwinContext();

            owinContext.Response.Cookies.Append("refresh_token" , tokenResult.RefreshToken , new CookieOptions
            {
                HttpOnly = true ,
                Secure = true ,
                SameSite = Microsoft.Owin.SameSiteMode.Lax ,
                Expires = DateTime.UtcNow.AddDays(NUMBER_CONSTANTS.REFRESH_TOKEN_EXPIRES_IN_DAYS) ,
                Path = "/api/auth/refresh"
            });

            return base.Ok(new LoginResponseDto
            {
                AccessToken = tokenResult.AccessToken ,
                ExpiresIn = NUMBER_CONSTANTS.JWT_EXPIRES_IN_SECONDS
            });
        }
    }
}
