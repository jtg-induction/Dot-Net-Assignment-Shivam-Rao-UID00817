using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System.Web.Http.Results;
using System.Web.Helpers;


namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Tests.Services
{
    [TestFixture]
    public class AuthServiceTest
    {
        private Mock<IUserRepository> _mockUserRepository;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _authService = new AuthService(_mockUserRepository.Object);
        }

        [Test]
        public async Task RegisterAsync_WhenUserDoesNotExist_AddsUser()
        {
            var model = new RegisterDto
            {
                Email = "    kwabersinked@blinklist.com" ,
                Name = "Kenny Wabersinke" ,
                Password = "mH9)dS1t" ,
                PhoneNumber = "561-662-8099"
            };

            _mockUserRepository.Setup(
                x => x.AddUserAsync(It.IsAny<Users>()))
                .Returns(Task.CompletedTask);
            
            await _authService.RegisterAsync(model);

            _mockUserRepository.Verify(
                x => x.UserExistsAsync(
                    "kwabersinked@blinklist.com" ,
                    "561-662-8099"),
                Times.Once
            );

            _mockUserRepository.Verify(
                x => x.AddUserAsync(It.IsAny<Users>()) ,
                Times.Once
            );
        }

        [Test]
        public async Task RegisterAsync_CreatesUsersWithCorrectDefaults()
        {
            var model = new RegisterDto
            {
                Email = "kwabersinked@blinklist.com" ,
                Name = "Kenny Wabersinke" ,
                Password = "mH9)dS1t" ,
                PhoneNumber = "561-662-8099"
            };

            _mockUserRepository.Setup(x => x.UserExistsAsync(
                "kwabersinked@blinklist.com" ,
                "561-662-8099")).ReturnsAsync(false);

            Users createdUser = null;

            _mockUserRepository.Setup(x => x.AddUserAsync(It.IsAny<Users>()))
                .Callback<Users>(user =>
                {
                    createdUser = user;
                })
                .Returns(Task.CompletedTask);

            await _authService.RegisterAsync(model);

            Assert.That(createdUser , Is.Not.Null);

            Assert.That(
                createdUser.Email ,
                Is.EqualTo("kwabersinked@blinklist.com")
            );

            Assert.That(
                createdUser.PhoneNumber,
                Is.EqualTo("561-662-8099")
            );

            Assert.That(
                createdUser.Name ,
                Is.EqualTo("Kenny Wabersinke")
            );

            Assert.That(
                createdUser.Role ,
                Is.EqualTo("Customer")
            );

            Assert.That(
                createdUser.WalletBalance ,
                Is.EqualTo(1000m)    
            );

            Assert.That(
                createdUser.IsActive,
                Is.True
            );
        }
    }
}
