using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Helpers;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ValidationException = Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions.ValidationException;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost, Route("register")]
        public async Task Register([FromBody] RegisterDto model)
        {
            if (model is null) throw new ValidationException(Constants.ExceptionMessages.MODEL_WAS_NULL);
            await _authService.RegisterAsync(model);
        }

        [HttpPost, Route("login")]
        public async Task<IHttpActionResult> Login([FromBody] LoginRequestDto model)
        {
            if (model is null) throw new ValidationException(Constants.ExceptionMessages.MODEL_WAS_NULL);

            AuthService.TokenResult tokenResult = (AuthService.TokenResult)await _authService.LoginAsync(model);

            CookieHelper.CreateHttpOnlySecureCookie("/api/auth/refresh" , tokenResult , Request);
            CookieHelper.CreateHttpOnlySecureCookie("/api/auth/logout" , tokenResult , Request);

            return base.Ok(new LoginResponseDto
            {
                AccessToken = tokenResult.AccessToken ,
                ExpiresIn = NumberConstants.JWT_EXPIRES_IN_SECONDS
            });
        }

        [HttpPost, Route("refresh")]
        public async Task<IHttpActionResult> Refresh()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["refresh_token"];

            if (cookie == null) return Unauthorized();

            AuthService.TokenResult tokenResult = (AuthService.TokenResult)await _authService.RotateTokenAsync(HttpUtility.UrlDecode(cookie.Value));

            CookieHelper.CreateHttpOnlySecureCookie("/api/auth/refresh" , tokenResult , Request);
            CookieHelper.CreateHttpOnlySecureCookie("/api/auth/logout" , tokenResult , Request);

            return base.Ok(new LoginResponseDto
            {
                AccessToken = tokenResult.AccessToken ,
                ExpiresIn = NumberConstants.JWT_EXPIRES_IN_SECONDS
            });
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
