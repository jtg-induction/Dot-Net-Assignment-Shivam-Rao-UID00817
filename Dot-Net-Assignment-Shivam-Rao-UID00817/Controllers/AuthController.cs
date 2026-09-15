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
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Helpers;

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
        public async Task Register([FromBody] RegisterDto model)
        {
            await _authService.RegisterAsync(model);
        }

        [HttpPost, Route("login")]
        public async Task<IHttpActionResult> Login([FromBody] LoginRequestDto model)
        {
            AuthService.TokenResult tokenResult = (AuthService.TokenResult) await _authService.LoginAsync(model);

            CookieHelper.CreateHttpOnlySecureCookie("/api/auth/refresh" , tokenResult , Request);

            owinContext.Response.Cookies.Append("refresh_token", tokenResult.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = Microsoft.Owin.SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(NUMBER_CONSTANTS.REFRESH_TOKEN_EXPIRES_IN_DAYS),
                Path = "/api/auth/logout"
            });

            return base.Ok(new LoginResponseDto
                {
                    AccessToken = tokenResult.AccessToken ,
                    ExpiresIn = NUMBER_CONSTANTS.JWT_EXPIRES_IN_SECONDS
            });
            }
            catch (Exceptions.ValidationException)
            {
                return Unauthorized();
            }
        }

        [HttpPost, Route("refresh")]
        public async Task<IHttpActionResult> Refresh()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["refresh_token"];

            if (cookie == null) return Unauthorized();

            AuthService.TokenResult tokenResult = (AuthService.TokenResult) await _authService.RotateTokenAsync(HttpUtility.UrlDecode(cookie.Value));

            CookieHelper.CreateHttpOnlySecureCookie("/api/auth/refresh" , tokenResult , Request);

            owinContext.Response.Cookies.Append("refresh_token", tokenResult.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = Microsoft.Owin.SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(NUMBER_CONSTANTS.REFRESH_TOKEN_EXPIRES_IN_DAYS),
                Path = "/api/auth/logout"
            });

            return base.Ok(new LoginResponseDto
                {
                    AccessToken = tokenResult.AccessToken ,
                    ExpiresIn = NUMBER_CONSTANTS.JWT_EXPIRES_IN_SECONDS
            });
            }
        }
        [Authorize]
        [HttpPost, Route("logout")]
        public async Task<IHttpActionResult> Logout()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["refresh_token"];

            if (cookie == null) return Unauthorized();

            if (await _authService.LogoutAsync(HttpUtility.UrlDecode(cookie.Value))) return Ok();

            return Unauthorized();
        }
    }
}
