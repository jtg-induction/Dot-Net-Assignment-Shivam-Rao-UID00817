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
using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using System.Web.Http;
using System.Net;


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
                        Constants.EXCEPTION_MESSAGES.USER_ALREADY_EXISTS
                    )
                );

            var exception = Assert.ThrowsAsync<ConflictException>(
                async () => await _controller.Register(model)
            );

            Assert.That(
                exception.Message ,
                Is.EqualTo(Constants.EXCEPTION_MESSAGES.USER_ALREADY_EXISTS)
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
                nameof(RegisterDto.Email) ,
                Constants.EXCEPTION_MESSAGES.INVALID_EMAIL_FORMAT
            );

            var exception = Assert.ThrowsAsync<ModelValidationException>(
                async () => await _controller.Register(model)
            );

            Assert.That(
                exception.Message ,
                Is.EqualTo(Constants.EXCEPTION_MESSAGES.INVALID_EMAIL_FORMAT)
            );

            _mockAuthService.Verify(
                s => s.RegisterAsync(It.IsAny<RegisterDto>()) ,
                Times.Never
            );
        }

    }
}
