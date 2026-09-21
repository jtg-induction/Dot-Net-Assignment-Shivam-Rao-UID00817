using Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;


namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Tests.Controllers
{
    [TestFixture]
    public class AuthControllerTest
    {
        private Mock<IAuthService> _mockAuthService;
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        private void SetHttpContext(HttpCookieCollection cookies)
        {
            var request = new HttpRequest(
                "" ,
                "http://localhost/api/auth/logout" ,
                ""
            );

            var response = new HttpResponse(null);

            var context = new HttpContext(request , response);

            foreach (string key in cookies)
            {
                context.Request.Cookies.Add(cookies[key]);
            }

            HttpContext.Current = context;
        }

        [Test]
        public async Task Register_ValidUser_ReturnsOk()
        {
            var model = new RegisterDto
            {
                Email = "kwabersinked@blinklist.com" ,
                Name = "Kenny Wabersinke" ,
                Password = "mH9)dS1t" ,
                PhoneNumber = "561-662-8099"
            };

            _mockAuthService.Setup(
                s => s.RegisterAsync(model))
                .Returns(Task.CompletedTask);

            await _controller.Register(model);

            _mockAuthService.Verify(
                x => x.RegisterAsync(model) ,
                Times.Once);
        }

        [Test]
        public async Task Register_DuplicateUser_ReturnsValidationException()
        {
            var model = new RegisterDto
            {
                Email = "ab10@example.com" ,
                Name = "Klena" ,
                Password = "mH9)dS1t" ,
                PhoneNumber = "823-452-3464"
            };

            _mockAuthService
                .Setup(s => s.RegisterAsync(model))
                .ThrowsAsync(
                    new ConflictException(
                        Constants.ErrorMessages.USER_ALREADY_EXISTS
                    )
                );

            var exception = Assert.ThrowsAsync<ConflictException>(
                async () => await _controller.Register(model)
            );

            Assert.That(
                exception.Message ,
                Is.EqualTo(Constants.ErrorMessages.USER_ALREADY_EXISTS)
            );
        }

        [Test]
        public async Task Logout_NoRefreshTokenCookie_ReturnsUnauthorized()
        {
            SetHttpContext(new HttpCookieCollection());

            IHttpActionResult result = await _controller.Logout();

            Assert.That(result , Is.TypeOf<UnauthorizedResult>());

            _mockAuthService.Verify(
                x => x.LogoutAsync(It.IsAny<string>()) ,
                Times.Never
            );
        }

        [Test]
        public async Task Logout_ValidRefreshToken_ReturnsOk()
        {
            string refreshToken = "abc123";

            var cookies = new HttpCookieCollection();
            cookies.Add(new HttpCookie("refresh_token" , refreshToken));

            SetHttpContext(cookies);

            _mockAuthService.Setup(
                x => x.LogoutAsync(refreshToken))
                .ReturnsAsync(true);

            IHttpActionResult result = await _controller.Logout();

            Assert.That(result , Is.TypeOf<OkResult>());

            _mockAuthService.Verify(
                x => x.LogoutAsync(refreshToken) ,
                Times.Once
            );
        }

        [Test]
        public async Task Logout_RefreshTokenDoesNotExist_ReturnsUnauthorized()
        {
            string refreshToken = "invalid-token";

            var cookies = new HttpCookieCollection();
            cookies.Add(new HttpCookie("refresh_token" , refreshToken));

            SetHttpContext(cookies);

            _mockAuthService.Setup(
                x => x.LogoutAsync(refreshToken))
                .ReturnsAsync(false);

            IHttpActionResult result = await _controller.Logout();

            Assert.That(result , Is.TypeOf<UnauthorizedResult>());

            _mockAuthService.Verify(
                x => x.LogoutAsync(refreshToken) ,
                Times.Once
            );
        }
    }
}
