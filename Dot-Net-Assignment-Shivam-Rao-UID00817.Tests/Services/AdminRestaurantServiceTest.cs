using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Tests.Services
{

    [TestFixture]
    public class AdminRestaurantServiceTest
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IRestaurantRepository> _mockRestaurantRepository;
        private Mock<IOwnerManagesRestaurantsRepository> _mockOwnerRepository;

        private AdminRestaurantService _service;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRestaurantRepository = new Mock<IRestaurantRepository>();
            _mockOwnerRepository = new Mock<IOwnerManagesRestaurantsRepository>();

            _service = new AdminRestaurantService(
                _mockUserRepository.Object ,
                _mockUnitOfWork.Object ,
                _mockRestaurantRepository.Object ,
                _mockOwnerRepository.Object
            );
        }

        [Test]
        public async Task GetValidEmailsAsync_ValidEmail_ReturnsNormalizedEmail()
        {
            var emails = new List<string> { "  OWNER@TEST.COM  " };

            var status = new List<EmailAndStatus>();

            var result = await _service.GetValidEmailsAsync(emails ,status);

            Assert.That(result.Count ,Is.EqualTo(1));
            Assert.That(result[0] ,Is.EqualTo("owner@test.com"));

            Assert.That(status.Count ,Is.EqualTo(0));
        }

        [Test]
        public async Task GetValidEmailsAsync_InvalidEmail_AddsFailedStatus()
        {
            var emails = new List<string> { "invalid-email" };

            var status = new List<EmailAndStatus>();

            var result = await _service.GetValidEmailsAsync(emails ,status);

            Assert.That(result.Count ,Is.EqualTo(0));
            Assert.That(status.Count ,Is.EqualTo(1));

            Assert.That(status[0].Email ,Is.EqualTo("invalid-email"));
            Assert.That(status[0].IsOnBoarded ,Is.False);
            Assert.That(
                status[0].Message ,
                Is.EqualTo(ErrorMessages.INVALID_EMAIL_FORMAT)
            );
        }

        [Test]
        public void OnboardRestaurantAsync_RestaurantAlreadyExists_ThrowsConflictException()
        {
            var restaurant = new RestaurantOnboardDto
            {
                Name = "Existing Restaurant" ,
                Emails = new List<string> { "owner@test.com" }
            };

            var existingRestaurant = new Restaurants(
                "Existing Restaurant" ,
                "Address" ,
                "Delhi" ,
                "Delhi" ,
                "110001" ,
                "India" ,
                ""
            );

            _mockRestaurantRepository.Setup(
                x => x.GetRestaurantAsync(
                    "Existing Restaurant" ,
                    false))
                .ReturnsAsync(existingRestaurant);

            var exception = Assert.ThrowsAsync<ConflictException>(
                async () => await _service.OnboardRestaurantAsync(restaurant)
            );

            Assert.That(
                exception.Message ,
                Is.EqualTo(ErrorMessages.RESTAURANT_ALREADY_EXISTS)
            );

            _mockRestaurantRepository.Verify(
                x => x.GetRestaurantAsync("Existing Restaurant" ,false) ,
                Times.Once
            );
        }

        [Test]
        public void OnboardRestaurantAsync_NoValidUsers_ThrowsValidationException()
        {
            var restaurant = new RestaurantOnboardDto
            {
                Name = "New Restaurant" ,
                Emails = new List<string> { "owner@test.com" }
            };

            _mockRestaurantRepository.Setup(
                x => x.GetRestaurantAsync(
                    "New Restaurant" ,
                    false))
                .ReturnsAsync((Restaurants)null);

            _mockUserRepository.Setup(
                x => x.GetUsersByEmails(
                    It.Is<List<string>>(emails => emails.Contains("owner@test.com"))))
                .ReturnsAsync(new List<Users>());

            var exception = Assert.ThrowsAsync<ValidationException>(
                async () => await _service.OnboardRestaurantAsync(restaurant)
            );

            Assert.That(
                exception.Message ,
                Is.EqualTo(ErrorMessages.NO_VALID_EMAILS)
            );

            _mockRestaurantRepository.Verify(
                x => x.Add(It.IsAny<Restaurants>()) ,
                Times.Never
            );

            _mockUnitOfWork.Verify(
                x => x.SaveChangesAsync() ,
                Times.Never
            );
        }

        [Test]
        public async Task OnboardRestaurantAsync_ValidRestaurant_ReturnsResponse()
        {
            var restaurant = new RestaurantOnboardDto
            {
                Name = "New Restaurant" ,
                AddressLine1 = "123 Main Street" ,
                City = "Delhi" ,
                State = "Delhi" ,
                Pincode = "110001" ,
                Country = "India" ,
                AddressLine2 = "" ,
                Emails = new List<string> { "owner@test.com" }
            };

            var user = new Users
            {
                UserId = 1 ,
                Email = "owner@test.com"
            };

            var createdRestaurant = new Restaurants(
                "New Restaurant" ,
                "123 Main Street" ,
                "Delhi" ,
                "Delhi" ,
                "110001" ,
                "India" ,
                ""
            )
            {
                RestaurantId = 10 ,
                IsActive = true
            };

            _mockRestaurantRepository
                .SetupSequence(x => x.GetRestaurantAsync(
                    "New Restaurant" ,
                    false))
                .ReturnsAsync((Restaurants)null)
                .ReturnsAsync(createdRestaurant);

            _mockUserRepository.Setup(
                x => x.GetUsersByEmails(
                    It.IsAny<List<string>>()))
                .ReturnsAsync(new List<Users> { user });

            _mockUnitOfWork.Setup(
                x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            var result = await _service.OnboardRestaurantAsync(restaurant);

            Assert.That(result ,Is.Not.Null);
            Assert.That(result.EmailStatus ,Is.Not.Null);

            _mockRestaurantRepository.Verify(
                x => x.Add(It.Is<Restaurants>(
                    r => r.Name == "New Restaurant"
                )) ,
                Times.Once
            );

            _mockUnitOfWork.Verify(
                x => x.SaveChangesAsync() ,
                Times.AtLeastOnce
            );

            _mockOwnerRepository.Verify(
                x => x.Add(It.IsAny<List<Owner_Manages_Restaurants>>()) ,
                Times.Once
            );
        }



        [Test]
        public async Task AssignOwnerToRestaurantAsync_ValidRequest_OnboardsUsers()
        {
            var model = new OwnerOnboardRequestDto
            {
                Name = "Test Restaurant" ,
                Emails = new List<string> { "owner@test.com" }
            };

            var user = new Users
            {
                UserId = 1 ,
                Email = "owner@test.com"
            };

            var restaurant = new Restaurants(
                "Test Restaurant" ,
                "Address" ,
                "Delhi" ,
                "Delhi" ,
                "110001" ,
                "India" ,
                ""
            )
            {
                RestaurantId = 10 ,
                IsActive = true
            };

            _mockUserRepository.Setup(
                x => x.GetUsersByEmails(
                    It.IsAny<List<string>>()))
                .ReturnsAsync(new List<Users> { user });

            _mockRestaurantRepository.Setup(
                x => x.GetRestaurantAsync(
                    "Test Restaurant" ,
                    false))
                .ReturnsAsync(restaurant);

            _mockUnitOfWork.Setup(
                x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            var result =
                await _service.AssignOwnerToRestaurantAsync(model);

            Assert.That(result ,Is.Not.Null);
            Assert.That(result.EmailStatus.Count ,Is.EqualTo(1));

            Assert.That(user.Role ,Is.EqualTo(Enums.Roles.Owner));

            _mockOwnerRepository.Verify(
                x => x.Add(
                    It.Is<List<Owner_Manages_Restaurants>>(
                        list => list.Count == 1
                    )
                ) ,
                Times.Once
            );

            _mockUnitOfWork.Verify(
                x => x.SaveChangesAsync() ,
                Times.Once
            );
        }


        [Test]
        public void AssignOwnerToRestaurantAsync_RestaurantDoesNotExist_ThrowsValidationException()
        {
            var model = new OwnerOnboardRequestDto
            {
                Name = "Missing Restaurant" ,
                Emails = new List<string> { "owner@test.com" }
            };

            var user = new Users
            {
                UserId = 1 ,
                Email = "owner@test.com"
            };

            _mockUserRepository.Setup(
                x => x.GetUsersByEmails(It.IsAny<List<string>>()))
                .ReturnsAsync(new List<Users> { user });

            _mockRestaurantRepository.Setup(
                x => x.GetRestaurantAsync(
                    "Missing Restaurant" ,
                    false))
                .ReturnsAsync((Restaurants)null);

            var exception = Assert.ThrowsAsync<ValidationException>(
                async () =>
                    await _service.AssignOwnerToRestaurantAsync(model)
            );

            Assert.That(
                exception.Message ,
                Is.EqualTo(ErrorMessages.RESTAURANT_DOES_NOT_EXIST)
            );

            _mockOwnerRepository.Verify(
                x => x.Add(It.IsAny<List<Owner_Manages_Restaurants>>()) ,
                Times.Never
            );

            _mockUnitOfWork.Verify(
                x => x.SaveChangesAsync() ,
                Times.Never
            );
        }


    }
}
