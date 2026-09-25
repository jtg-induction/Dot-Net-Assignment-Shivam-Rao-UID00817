using Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Tests.Controllers
{
    [TestFixture]
    public class OrderControllerTest
    {
        private Mock<IOrderService> _mockOrderService;
        private OrderController _controller;

        [SetUp]
        public void Setup()
        {
            _mockOrderService = new Mock<IOrderService>();

            _controller = new OrderController(
                _mockOrderService.Object
            );

            var config = new HttpConfiguration();

            var request = new HttpRequestMessage(
                HttpMethod.Get ,
                "http://localhost/api/orders"
            );

            request.SetConfiguration(config);

            _controller.Request = request;
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
        public async Task PlaceOrder_ValidRequest_ReturnsCreated()
        {
            long userId = 1;
            var model = new OrderRequestDto();
            var serviceResponse = new OrderResponseDto();

            SetUser(userId);

            _mockOrderService
                .Setup(x => x.PlaceOrderAsync(userId ,model))
                .ReturnsAsync(serviceResponse);

            var response = await _controller.PlaceOrder(model);

            Assert.AreEqual(
                HttpStatusCode.Created ,
                response.StatusCode
            );

            _mockOrderService.Verify(
                x => x.PlaceOrderAsync(userId ,model) ,
                Times.Once
            );
        }

        [Test]
        public async Task GetCustomerOrders_ValidRequest_ReturnsOk()
        {
            long userId = 1;
            int pageNumber = 2;

            SetUser(userId);

            _mockOrderService
                .Setup(x => x.GetAllOrdersAsync(userId ,pageNumber))
                .ReturnsAsync(new GetCustomerOrdersDto());

            var response =
                await _controller.GetCustomerOrders(pageNumber);

            Assert.AreEqual(
                HttpStatusCode.OK ,
                response.StatusCode
            );

            _mockOrderService.Verify(
                x => x.GetAllOrdersAsync(userId ,pageNumber) ,
                Times.Once
            );
        }

        [Test]
        public async Task GetOrderDetails_ValidRequest_ReturnsOk()
        {
            long userId = 1;
            long orderId = 100;

            SetUser(userId);

            _mockOrderService
                .Setup(x => x.GetOrderDetailsAsync(userId ,orderId))
                .ReturnsAsync(new GetCustomerOrderDetailsDto());

            var response =
                await _controller.GetOrderDetails(orderId);

            Assert.AreEqual(
                HttpStatusCode.OK ,
                response.StatusCode
            );

            _mockOrderService.Verify(
                x => x.GetOrderDetailsAsync(userId ,orderId) ,
                Times.Once
            );
        }

        [Test]
        public async Task CancelOrder_ValidRequest_ReturnsNoContent()
        {
            long userId = 1;
            long orderId = 100;

            SetUser(userId);

            _mockOrderService
                .Setup(x => x.CancelOrderAsync(userId ,orderId))
                .Returns(Task.CompletedTask);

            var response =
                await _controller.CancelOrder(orderId);

            Assert.AreEqual(
                HttpStatusCode.NoContent ,
                response.StatusCode
            );

            _mockOrderService.Verify(
                x => x.CancelOrderAsync(userId ,orderId) ,
                Times.Once
            );
        }
    }
}
