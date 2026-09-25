using Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Tests.Controllers
{

    [TestFixture]
    public class AdminRestaurantControllerTest
    {
        private Mock<IAdminRestaurantService> _mockAdminService;
        private AdminRestaurantController _controller;

        [SetUp]
        public void Setup()
        {
            _mockAdminService = new Mock<IAdminRestaurantService>();

            _controller = new AdminRestaurantController(
                _mockAdminService.Object
            );

            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new System.Web.Http.HttpConfiguration();
        }

        [Test]
        public async Task Restaurant_ValidRequest_ReturnsOk()
        {
            var model = new RestaurantOnboardDto
            {
                Name = "Test Restaurant" ,
                AddressLine1 = "123 Main Street" ,
                City = "Delhi" ,
                State = "Delhi" ,
                Pincode = "110001" ,
                Country = "India" ,
                Emails = ["owner@test.com"]
            };

            var expectedResponse = new OwnerOnboardResponseDto
            {
                EmailStatus = new List<EmailAndStatus>()
            };

            _mockAdminService.Setup(
                x => x.OnboardRestaurantAsync(model))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.Restaurant(model);

            Assert.That(result ,Is.Not.Null);
            Assert.That(result.StatusCode ,Is.EqualTo(HttpStatusCode.OK));

            _mockAdminService.Verify(
                x => x.OnboardRestaurantAsync(model) ,
                Times.Once
            );
        }


        [Test]
        public async Task Onboard_ValidRequest_ReturnsOk()
        {
            var model = new OwnerOnboardRequestDto
            {
                Name = "Test Restaurant" ,
                Emails = ["owner@test.com"]
            };

            var expectedResponse = new OwnerOnboardResponseDto
            {
                EmailStatus = new List<EmailAndStatus>()
            };

            _mockAdminService.Setup(
                x => x.AssignOwnerToRestaurantAsync(model))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.Onboard(model);

            Assert.That(result ,Is.Not.Null);
            Assert.That(result.StatusCode ,Is.EqualTo(HttpStatusCode.OK));

            _mockAdminService.Verify(
                x => x.AssignOwnerToRestaurantAsync(model) ,
                Times.Once
            );
        }


        [Test]
        public async Task Deactivate_ValidRequest_ReturnsNoContent()
        {
            var model = new RestaurantActivateDeactivateDto
            {
                Name = "Test Restaurant"
            };

            _mockAdminService.Setup(
                x => x.DeactivateRestaurant(model.Name))
                .Returns(Task.CompletedTask);

            var result = await _controller.Deactivate(model);

            Assert.That(result ,Is.TypeOf<StatusCodeResult>());

            var statusResult = (StatusCodeResult)result;

            Assert.That(
                statusResult.StatusCode ,
                Is.EqualTo(HttpStatusCode.NoContent)
            );

            _mockAdminService.Verify(
                x => x.DeactivateRestaurant(model.Name) ,
                Times.Once
            );
        }


        [Test]
        public async Task Activate_ValidRequest_ReturnsNoContent()
        {
            var model = new RestaurantActivateDeactivateDto
            {
                Name = "Test Restaurant"
            };

            _mockAdminService.Setup(
                x => x.ActivateRestaurant(model.Name))
                .Returns(Task.CompletedTask);

            var result = await _controller.Activate(model);

            Assert.That(result ,Is.TypeOf<StatusCodeResult>());

            var statusResult = (StatusCodeResult)result;

            Assert.That(
                statusResult.StatusCode ,
                Is.EqualTo(HttpStatusCode.NoContent)
            );

            _mockAdminService.Verify(
                x => x.ActivateRestaurant(model.Name) ,
                Times.Once
            );
        }
    }
}
