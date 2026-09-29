using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Collections.Generic;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Tests.Controllers
{

    [TestFixture]
    public class RestaurantOwnerControllerTest
    {
        private Mock<IOrderService> _mockOrderService;
        private Mock<IOwnerManagesRestaurantsRepository> _mockOwnerManagesRestaurantsRepository;
        private RestaurantOwnerController _controller;

        [SetUp]
        public void Setup()
        {
            _mockOrderService = new Mock<IOrderService>();

            _controller = new RestaurantOwnerController(
                _mockOrderService.Object
            );

            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            SetUser(101);
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
        public async Task GetRestaurantOrders_ValidRequest_ReturnsOk()
        {
            long restaurantId = 500;
            int pageNumber = 2;
            int pageSize = 5;
            string search = "452";
            string status = Status.ACCEPTED;
            var sortBy = "order-date";
            var sortDirection = Enums.SortDirection.DESC;
            string City = "Delhi";

            _mockOrderService
                .Setup(x => x.GetAllOrdersAsync(
                    101 ,
                    restaurantId ,
                    status , 
                    pageNumber ,
                    pageSize,
                    search,
                    sortBy ,
                    sortDirection,
                    City))
                .ReturnsAsync(new GetRestaurantOrdersDto { Orders = new List<RestaurantOrderHistoryItems> { new RestaurantOrderHistoryItems() } });

            var response = await _controller.GetRestaurantOrders(
                restaurantId ,
                    pageNumber ,
                    pageSize ,
                    search ,
                    sortBy ,
                    sortDirection, 
                    status ,
                    City);

            Assert.That(
                response.StatusCode ,
                Is.EqualTo(HttpStatusCode.OK));

            _mockOrderService.Verify(
                x => x.GetAllOrdersAsync(
                    101 ,
                    restaurantId ,
                    status,
                    pageNumber ,
                    pageSize ,
                    search ,
                    sortBy ,
                    sortDirection,
                    City) ,
                Times.Once);
        }

        [Test]
        public async Task GetOrderDetails_ValidRequest_ReturnsOk()
        {
            long restaurantId = 500;
            long orderId = 9001;

            _mockOrderService
                .Setup(x => x.GetOrderDetailsAsync(
                    101 ,
                    restaurantId ,
                    orderId))
                .ReturnsAsync(new GetRestaurantOrderDetailsDto());

            var response = await _controller.GetOrderDetails(
                restaurantId ,
                orderId);

            Assert.That(
                response.StatusCode ,
                Is.EqualTo(HttpStatusCode.OK));

            _mockOrderService.Verify(
                x => x.GetOrderDetailsAsync(
                    101 ,
                    restaurantId ,
                    orderId) ,
                Times.Once);
        }

        [Test]
        public async Task ManageOrder_ValidRequest_ReturnsNoContent()
        {

            long restaurantId = 500;
            long orderId = 9001;

            var model = new OrderManagementRequestDto
            {
                ChangeStatusTo = Enums.OrderStatus.Accepted
            };

            _mockOrderService
                .Setup(x => x.ManageOrderAsync(
                    restaurantId ,
                    101 ,
                    orderId ,
                    model))
                .Returns(Task.CompletedTask);

            var response = await _controller.ManageOrder(
                restaurantId ,
                orderId ,
                model);

            Assert.That(
                response.StatusCode ,
                Is.EqualTo(HttpStatusCode.NoContent));

            _mockOrderService.Verify(
                x => x.ManageOrderAsync(
                    restaurantId ,
                    101 ,
                    orderId ,
                    model) ,
                Times.Once);
        }
    }
}
