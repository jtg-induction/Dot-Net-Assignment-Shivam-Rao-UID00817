using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Tests.Services
{

    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockUserRespository;
        private Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IAuthService> _mockAuthService;

        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            _mockUserRespository = new Mock<IUserRepository>();
            _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockAuthService = new Mock<IAuthService>();

            _userService = new UserService(
                _mockUserRespository.Object ,
                _mockRefreshTokenRepository.Object ,
                _mockUnitOfWork.Object ,
                _mockAuthService.Object
            );
        }

        [Test]
        public async Task DeactivateAccountAsync_ValidUser_DeactivatesUserLogsOutAndSaves()
        {
            long userId = 123;

            _mockUserRespository.Setup(
                x => x.DeactivateUserAsync(userId))
                .Returns(Task.CompletedTask);

            _mockAuthService.Setup(
                x => x.LogOutFromAllDevicesAsync(userId))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(
                x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            await _userService.DeactivateAccountAsync(userId);

            _mockUserRespository.Verify(
                x => x.DeactivateUserAsync(userId) ,
                Times.Once
            );

            _mockAuthService.Verify(
                x => x.LogOutFromAllDevicesAsync(userId) ,
                Times.Once
            );

            _mockUnitOfWork.Verify(
                x => x.SaveChangesAsync() ,
                Times.Once
            );
        }

        [Test]
        public async Task UpdateAccountAsync_NullModel_DoesNothing()
        {
            long userId = 123;

            await _userService.UpdateAccountAsync(userId ,null);

            _mockUserRespository.Verify(
                x => x.GetUserByUserIdAsync(It.IsAny<long>() ,It.IsAny<bool>()) ,
                Times.Never
            );

            _mockUnitOfWork.Verify(
                x => x.SaveChangesAsync() ,
                Times.Never
            );
        }

        [Test]
        public async Task UpdateAccountAsync_NameProvided_TrimsAndUpdatesName()
        {
            long userId = 123;

            var user = new Users
            {
                UserId = userId ,
                Name = "Old Name" ,
                PhoneNumber = "9999999999"
            };

            var model = new UpdateAccountDto
            {
                Name = "  John  "
            };

            _mockUserRespository.Setup(
                x => x.GetUserByUserIdAsync(userId ,true))
                .ReturnsAsync(user);

            _mockUnitOfWork.Setup(
                x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            await _userService.UpdateAccountAsync(userId ,model);

            Assert.That(user.Name ,Is.EqualTo("John"));

            _mockUnitOfWork.Verify(
                x => x.SaveChangesAsync() ,
                Times.Once
            );
        }

        [Test]
        public async Task UpdateAccountAsync_NewPhoneNumber_UpdatesPhoneNumber()
        {
            long userId = 123;

            var user = new Users
            {
                UserId = userId ,
                Name = "John" ,
                PhoneNumber = "9999999999"
            };

            var model = new UpdateAccountDto
            {
                PhoneNumber = "  9876543210  "
            };

            _mockUserRespository.Setup(
                x => x.GetUserByUserIdAsync(userId ,true))
                .ReturnsAsync(user);

            _mockUserRespository.Setup(
                x => x.PhoneNumberExistsAsync("9876543210"))
                .ReturnsAsync(false);

            _mockUnitOfWork.Setup(
                x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            await _userService.UpdateAccountAsync(userId ,model);

            Assert.That(
                user.PhoneNumber ,
                Is.EqualTo("9876543210")
            );

            _mockUserRespository.Verify(
                x => x.PhoneNumberExistsAsync("9876543210") ,
                Times.Once
            );

            _mockUnitOfWork.Verify(
                x => x.SaveChangesAsync() ,
                Times.Once
            );
        }

        [Test]
        public void UpdateAccountAsync_DuplicatePhoneNumber_ThrowsConflictException()
        {
            long userId = 123;

            var user = new Users
            {
                UserId = userId ,
                Name = "John" ,
                PhoneNumber = "9999999999"
            };

            var model = new UpdateAccountDto
            {
                PhoneNumber = " 9876543210 "
            };

            _mockUserRespository.Setup(
                x => x.GetUserByUserIdAsync(userId ,true))
                .ReturnsAsync(user);

            _mockUserRespository.Setup(
                x => x.PhoneNumberExistsAsync("9876543210"))
                .ReturnsAsync(true);

            var exception = Assert.ThrowsAsync<ConflictException>(
                async () => await _userService.UpdateAccountAsync(userId ,model)
            );

            Assert.That(
                exception.Message ,
                Is.EqualTo(ErrorMessages.DUPLICATE_PHONE_NUMBER)
            );

            _mockUnitOfWork.Verify(
                x => x.SaveChangesAsync() ,
                Times.Never
            );
        }

        [Test]
        public async Task UpdateAccountAsync_NameAndPhoneProvided_UpdatesBoth()
        {
            long userId = 123;

            var user = new Users
            {
                UserId = userId ,
                Name = "Old Name" ,
                PhoneNumber = "9999999999"
            };

            var model = new UpdateAccountDto
            {
                Name = "  John Smith  " ,
                PhoneNumber = " 9876543210 "
            };

            _mockUserRespository.Setup(
                x => x.GetUserByUserIdAsync(userId ,true))
                .ReturnsAsync(user);

            _mockUserRespository.Setup(
                x => x.PhoneNumberExistsAsync("9876543210"))
                .ReturnsAsync(false);

            _mockUnitOfWork.Setup(
                x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            await _userService.UpdateAccountAsync(userId ,model);

            Assert.Multiple(() =>
            {
                Assert.That(user.Name ,Is.EqualTo("John Smith"));
                Assert.That(user.PhoneNumber ,Is.EqualTo("9876543210"));
            });

            _mockUnitOfWork.Verify(
                x => x.SaveChangesAsync() ,
                Times.Once
            );
        }
    }
}
