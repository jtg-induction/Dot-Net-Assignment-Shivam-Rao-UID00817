using Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using Moq;
using NUnit.Framework;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> _mockUserService;
        private UserController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        private void SetUser(long userId)
        {
            var claims = new[]
            {
            new Claim("userId", userId.ToString())
        };

            var identity = new ClaimsIdentity(
                claims ,
                "TestAuthentication"
            );

            _controller.User = new ClaimsPrincipal(identity);
        }

        [Test]
        public async Task Deactivate_ValidUser_CallsDeactivateAccount()
        {
            long userId = 123;

            SetUser(userId);

            _mockUserService.Setup(
                x => x.DeactivateAccountAsync(userId))
                .Returns(Task.CompletedTask);

            await _controller.Deactivate();

            _mockUserService.Verify(
                x => x.DeactivateAccountAsync(userId) ,
                Times.Once
            );
        }

        [Test]
        public async Task Update_ValidModel_CallsUpdateAccount()
        {
            long userId = 123;

            var model = new UpdateAccountDto
            {
                Name = "John" ,
                PhoneNumber = "9876543210"
            };

            SetUser(userId);

            _mockUserService
                .Setup(x => x.UpdateAccountAsync(userId , model))
                .Returns(Task.CompletedTask);

            await _controller.Update(model);
            
            _mockUserService.Verify(
                x => x.UpdateAccountAsync(userId , model) ,
                Times.Once
            );
        }
    }
}
