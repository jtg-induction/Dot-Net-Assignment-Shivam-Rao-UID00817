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
            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _authService.RegisterAsync(model);

                return Ok(new MessageResponseDto{ Message = "Registration successful!" });
            }
            catch (UserAlreadyExistsException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("login")]
        public async Task<IHttpActionResult> Login([FromBody] LoginRequestDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var tokenResult = await _authService.LoginAsync(model);

                IOwinContext owinContext = Request.GetOwinContext();

                owinContext.Response.Cookies.Append("refresh_token" , tokenResult.RefreshToken, new CookieOptions
                {
                    HttpOnly = true ,
                    Secure = true ,
                    SameSite = Microsoft.Owin.SameSiteMode.Lax ,
                    Expires = DateTime.UtcNow.AddDays(10) ,
                    Path = "/api/auth/refresh"
                });

                return Ok(new LoginResponseDto
                {
                    AccessToken = tokenResult.AccessToken ,
                    ExpiresIn = 900
                });
            }
            catch (InvalidCredentialsException)
            {
                return Unauthorized();
            }
        }

        [HttpPost, Route("refresh")]
        public async Task<IHttpActionResult> Refresh()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["refresh_token"];

            if (cookie == null) return Unauthorized();
            try
            {
                var tokenResult = await _authService.RotateTokenAsync(HttpUtility.UrlDecode(cookie.Value));

                if (tokenResult == null) return Unauthorized();

                IOwinContext owinContext = Request.GetOwinContext();

                owinContext.Response.Cookies.Append("refresh_token" , tokenResult.RefreshToken , new CookieOptions
                {
                    HttpOnly = true ,
                    Secure = true ,
                    SameSite = Microsoft.Owin.SameSiteMode.Lax ,
                    Expires = DateTime.UtcNow.AddDays(10) ,
                    Path = "/api/auth/refresh"
                });

                return Ok(new LoginResponseDto
                {
                    AccessToken = tokenResult.AccessToken ,
                    ExpiresIn = 900
                });
            }
            catch (InvalidCredentialsException)
            {
                return Unauthorized();
            }
        }
    }
}
