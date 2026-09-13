using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;
using System.Web.Helpers;


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

        [Test]
        public async Task Register_ValidUser_ReturnsOk()
        {
            var model = new RegisterDto {
                Email = "kwabersinked@blinklist.com" , 
                Name = "Kenny Wabersinke" , 
                Password = "mH9)dS1t" , 
                PhoneNumber = "561-662-8099" 
            };

            _mockAuthService.Setup(
                s => s.RegisterAsync(model))
                .Returns(Task.CompletedTask);

            var result = await _controller.Register(model);

            var okResult = result as OkNegotiatedContentResult<MessageResponseDto>;

            Assert.That(okResult , Is.Not.Null);

            Assert.That(
                okResult.Content.Message ,
                Is.EqualTo("Registration successful!"));

            _mockAuthService.Verify(
                x => x.RegisterAsync(model) ,
                Times.Once);
        }

        [Test]

        public async Task Register_DuplicateUser_ReturnsUserAlreadyExistsException()
        {
            var model = new RegisterDto { 
                Email = "ab10@example.com" , 
                Name = "Klena" , 
                Password = "mH9)dS1t" , 
                PhoneNumber = "823-452-3464" 
            };

            _mockAuthService.Setup(
                s => s.RegisterAsync(model))
                .ThrowsAsync(new UserAlreadyExistsException(
                    "Email / Phone Number already exists."
                ));

            var result = await _controller.Register(model);

            Assert.That(
                result , 
                Is.TypeOf<BadRequestErrorMessageResult>()
            );

        }

        [Test]
        public async Task Register_WhenModelStateIsInvalid_ReturnsBadRequest()
        {
            var model = new RegisterDto
            {
                Email = "invalidemail"
            };

            _controller.ModelState.AddModelError(
                "Email" ,
                "Invalid email address"
            );

            var result = await _controller.Register(model);

            Assert.That(
                result ,
                Is.TypeOf<InvalidModelStateResult>()
             );

            _mockAuthService.Verify(
                s => s.RegisterAsync(It.IsAny<RegisterDto>()),
                Times.Never
            );
        }
    }
}
