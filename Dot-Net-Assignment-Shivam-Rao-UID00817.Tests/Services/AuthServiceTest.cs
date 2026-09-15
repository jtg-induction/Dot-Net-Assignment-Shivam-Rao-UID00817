using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Utils;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http.Results;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;


namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Tests.Services
{
    [TestFixture]
    public class AuthServiceTest
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            Environment.SetEnvironmentVariable(
                "JWT_SECRET" ,
                "this-is-a-test-secret-key-123456789"
            );

            Environment.SetEnvironmentVariable(
                "JWT_ISSUER" ,
                "test-issuer"
            );

            Environment.SetEnvironmentVariable(
                "JWT_AUDIENCE" ,
                "test-audience"
            );

            _mockUserRepository = new Mock<IUserRepository>();
            _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _authService = new AuthService(_mockUserRepository.Object , _mockRefreshTokenRepository.Object);
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
                    "561-662-8099") ,
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
                createdUser.PhoneNumber ,
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
                createdUser.IsActive ,
                Is.True
            );
        }

        [Test]
        public async Task LoginAsync_CredentialsValid_ReturnsTokens()
        {
            var user = new Users
            {
                UserId = 1 ,
                Email = "shivam@example.com" ,
                Password = PasswordHasher.HashPassword("Pass@1234") ,
                Role = "Customer"
            };

            var model = new LoginRequestDto
            {
                Email = "shivam@example.com" ,
                Password = "Pass@1234"
            };

            _mockUserRepository.Setup(
                x => x.GetUserByEmailAsync("shivam@example.com"))
                .ReturnsAsync(user);

            var result = await _authService.LoginAsync(model);

            Assert.That(result , Is.Not.Null);
            Assert.That(result.AccessToken , Is.Not.Null.And.Not.Empty);
            Assert.That(result.RefreshToken , Is.Not.Null.And.Not.Empty);

            _mockRefreshTokenRepository.Verify(
                x => x.AddTokenAsync(
                    It.Is<Refresh_Tokens>(t =>
                            t.UserId == user.UserId &&
                            t.RefreshToken == result.RefreshToken
                    )) ,
                Times.Once
            );
        }

        [Test]
        public async Task LoginAsync_InvalidEmail_ThrowsInvalidCredentialsException()
        {
            var model = new LoginRequestDto
            {
                Email = "janedoe@example.com" ,
                Password = "Pass@1234"
            };

            _mockUserRepository.Setup(
                x => x.GetUserByEmailAsync("janedoe@example.com"))
                .ReturnsAsync((Users)null);

            Assert.ThrowsAsync<Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions.InvalidCredentialsException>(
                async () => await _authService.LoginAsync(model)
            );
        }

        [Test]
        public async Task LoginAsync_InvalidPassword_ThrowsInvalidCredentialsException()
        {
            var user = new Users
            {
                UserId = 1 ,
                Email = "Shivam@example.com" ,
                Password = PasswordHasher.HashPassword("Pass@1234") ,
                Role = "Customer"
            };

            var model = new LoginRequestDto
            {
                Email = "Shivam@Example.com" ,
                Password = "Invalid@1234"
            };

            _mockUserRepository.Setup(
                x => x.GetUserByEmailAsync(user.Email))
                .ReturnsAsync(user);

            Assert.ThrowsAsync<Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions.InvalidCredentialsException>(
                async () => await _authService.LoginAsync(model)
            );
        }

        [Test]
        public async Task RotateTokenAsync_ValidRefreshToken_ReturnsNewTokens()
        {
            var oldRefreshToken = "old-refresh-token";

            var tokenRecord = new Refresh_Tokens
            {
                RefreshToken = oldRefreshToken ,
                UserId = 1 ,
                CreatedAt = DateTime.UtcNow.AddDays(-1) ,
                ExpiresAt = DateTime.UtcNow.AddDays(9)
            };

            var user = new Users
            {
                UserId = 1 ,
                Email = "Shivam@example.com" ,
                Role = "Customer"
            };

            _mockRefreshTokenRepository
                .Setup(x => x.ValidateTokenAndGetDetails(oldRefreshToken))
                .ReturnsAsync(tokenRecord);

            _mockUserRepository
                .Setup(x => x.GetUserByUserIdAsync(1))
                .ReturnsAsync(user);

            var result = await _authService.RotateTokenAsync(oldRefreshToken);

            Assert.That(result , Is.Not.Null);
            Assert.That(result.AccessToken , Is.Not.Null.And.Not.Empty);
            Assert.That(result.RefreshToken , Is.Not.Null.And.Not.Empty);

            Assert.That(result.RefreshToken , Is.Not.EqualTo(oldRefreshToken));
        }

        [Test]
        public async Task RotateTokenAsync_ValidRefreshToken_StoresNewRefreshToken()
        {
            var oldRefreshToken = "old-refresh-token";

            var tokenRecord = new Refresh_Tokens
            {
                RefreshToken = oldRefreshToken ,
                UserId = 1 ,
                CreatedAt = DateTime.UtcNow.AddDays(-1) ,
                ExpiresAt = DateTime.UtcNow.AddDays(9)
            };

            var user = new Users
            {
                UserId = 1 ,
                Email = "Shivam@example.com" ,
                Role = "Customer"
            };

            _mockRefreshTokenRepository.Setup(
                x => x.ValidateTokenAndGetDetails(oldRefreshToken))
                .ReturnsAsync(tokenRecord);

            _mockUserRepository.Setup(
                x => x.GetUserByUserIdAsync(1))
                .ReturnsAsync(user);

            var result = await _authService.RotateTokenAsync(oldRefreshToken);

            Assert.That(result , Is.Not.Null);

            _mockRefreshTokenRepository.Verify(
                x => x.AddTokenAsync(
                    It.Is<Refresh_Tokens>(t =>
                        t.UserId == user.UserId &&
                        t.RefreshToken == result.RefreshToken
                    )) ,
                Times.Once
            );
        }

        [Test]
        public async Task RotateTokenAsync_InvalidRefreshToken_ReturnsNull()
        {
            var refreshToken = "invalid-token";

            _mockRefreshTokenRepository.Setup(
                x => x.ValidateTokenAndGetDetails(refreshToken))
                .ReturnsAsync((Refresh_Tokens)null);

            var result = await _authService.RotateTokenAsync(refreshToken);

            Assert.That(result , Is.Null);

            _mockUserRepository.Verify(
                x => x.GetUserByUserIdAsync(It.IsAny<long>()) ,
                Times.Never
            );

            _mockRefreshTokenRepository.Verify(
                x => x.AddTokenAsync(It.IsAny<Refresh_Tokens>()) ,
                Times.Never
            );
        }


        [Test]
        public async Task RotateTokenAsync_UserDoesNotExist_ReturnsNull()
        {
            var refreshToken = "valid-refresh-token";

            var tokenRecord = new Refresh_Tokens
            {
                RefreshToken = refreshToken ,
                UserId = 999 ,
                CreatedAt = DateTime.UtcNow.AddDays(-1) ,
                ExpiresAt = DateTime.UtcNow.AddDays(9)
            };

            _mockRefreshTokenRepository.Setup(
                x => x.ValidateTokenAndGetDetails(refreshToken))
                .ReturnsAsync(tokenRecord);

            _mockUserRepository.Setup(
                x => x.GetUserByUserIdAsync(999))
                .ReturnsAsync((Users)null);

            var result = await _authService.RotateTokenAsync(refreshToken);

            Assert.That(result , Is.Null);

            _mockRefreshTokenRepository.Verify(
                x => x.AddTokenAsync(It.IsAny<Refresh_Tokens>()) ,
                Times.Never
            );
        }
    }
}
