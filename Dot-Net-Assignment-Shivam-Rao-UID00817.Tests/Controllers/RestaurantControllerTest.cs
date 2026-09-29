using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Tests.Controllers
{
    [TestFixture]
    public class RestaurantControllerTests
    {
        private Mock<IRestaurantService> _restaurantServiceMock;
        private RestaurantController _controller;

        [SetUp]
        public void Setup()
        {
            _restaurantServiceMock = new Mock<IRestaurantService>();

            _controller = new RestaurantController(
                _restaurantServiceMock.Object
            );

            _controller.Request = new HttpRequestMessage();
        }

        [Test]
        public async Task Restaurant_ValidParameters_ReturnsOk()
        {
            int pageNumber = 1;
            int pageSize = NumberConstants.PAGE_SIZE;
            var expectedResult = new RestaurantsList { Restaurants = new List<GetRestaurantsResponseDto> { new GetRestaurantsResponseDto() } };

            _restaurantServiceMock
                .Setup(x => x.GetRestaurantsAsync(pageNumber ,pageSize))
                .ReturnsAsync(expectedResult);

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage();
            request.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = config;

            _controller.Request = request;
            _controller.Configuration = config;

            var response = await _controller.Restaurant(pageNumber ,pageSize);

            Assert.That(response ,Is.Not.Null);
            Assert.That(response.StatusCode ,Is.EqualTo(HttpStatusCode.OK));

            _restaurantServiceMock.Verify(x => x.GetRestaurantsAsync(pageNumber ,pageSize) ,Times.Once);
        }


        [Test]
        public async Task Restaurant_InvalidPageNumber_ReturnsNotFound()
        {
            int pageNumber = 0;
            int pageSize = NumberConstants.PAGE_SIZE;

            var response = await _controller.Restaurant(
                pageNumber ,
                pageSize
            );
            Assert.That(response ,Is.Not.Null);
            Assert.That(
                response.StatusCode ,
                Is.EqualTo(HttpStatusCode.NotFound)
            );

            _restaurantServiceMock.Verify(
                x => x.GetRestaurantsAsync(
                    It.IsAny<int>() ,
                    It.IsAny<int>()
                ) ,
                Times.Never
            );
        }

        [Test]
        public async Task Restaurant_InvalidPageSize_ReturnsNotFound()
        {
            int pageNumber = 1;
            int pageSize = 0;

            var response = await _controller.Restaurant(
                pageNumber ,
                pageSize
            );

            Assert.That(response ,Is.Not.Null);
            Assert.That(
                response.StatusCode ,
                Is.EqualTo(HttpStatusCode.NotFound)
            );

            _restaurantServiceMock.Verify(
                x => x.GetRestaurantsAsync(
                    It.IsAny<int>() ,
                    It.IsAny<int>()
                ) ,
                Times.Never
            );
        }

        [Test]
        public async Task Restaurant_PassesCorrectParametersToService()
        {
            int pageNumber = 3;
            int pageSize = 10;

            var expectedResult = new RestaurantsList();

            _restaurantServiceMock
                .Setup(x => x.GetRestaurantsAsync(pageNumber ,pageSize))
                .ReturnsAsync(expectedResult);

            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            await _controller.Restaurant(pageNumber ,pageSize);

            _restaurantServiceMock.Verify(
                x => x.GetRestaurantsAsync(pageNumber ,pageSize) ,
                Times.Once
            );
        }


        [Test]
        public async Task Menu_ValidParameters_ReturnsOk()
        {
            long restaurantId = 1;
            int pageNumber = 1;
            int pageSize = NumberConstants.PAGE_SIZE;

            var expectedResult = new GetMenuResponseDto { items = new List<ItemAndPrice> { new ItemAndPrice() } };

            _restaurantServiceMock
                .Setup(x => x.GetItemsAsync(
                    restaurantId ,
                    pageNumber ,
                    pageSize))
                .ReturnsAsync(expectedResult);

            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            var response = await _controller.Menu(
                restaurantId ,
                pageNumber ,
                pageSize
            );

            Assert.That(response ,Is.Not.Null);
            Assert.That(
                response.StatusCode ,
                Is.EqualTo(HttpStatusCode.OK)
            );

            _restaurantServiceMock.Verify(
                x => x.GetItemsAsync(
                    restaurantId ,
                    pageNumber ,
                    pageSize) ,
                Times.Once
            );
        }


        [Test]
        public async Task Menu_InvalidPageNumber_ReturnsNotFound()
        {
            long restaurantId = 1;
            int pageNumber = 0;
            int pageSize = NumberConstants.PAGE_SIZE;

            var response = await _controller.Menu(
                restaurantId ,
                pageNumber ,
                pageSize
            );

            Assert.That(response ,Is.Not.Null);
            Assert.That(
                response.StatusCode ,
                Is.EqualTo(HttpStatusCode.NotFound)
            );

            _restaurantServiceMock.Verify(
                x => x.GetItemsAsync(
                    It.IsAny<long>() ,
                    It.IsAny<int>() ,
                    It.IsAny<int>()) ,
                Times.Never
            );
        }

        [Test]
        public async Task Menu_InvalidPageSize_ReturnsNotFound()
        {
            long restaurantId = 1;
            int pageNumber = 1;
            int pageSize = 0;

            var response = await _controller.Menu(
                restaurantId ,
                pageNumber ,
                pageSize
            );

            Assert.That(response ,Is.Not.Null);
            Assert.That(
                response.StatusCode ,
                Is.EqualTo(HttpStatusCode.NotFound)
            );

            _restaurantServiceMock.Verify(
                x => x.GetItemsAsync(
                    It.IsAny<long>() ,
                    It.IsAny<int>() ,
                    It.IsAny<int>()) ,
                Times.Never
            );
        }

        [Test]
        public async Task Menu_PassesCorrectParametersToService()
        {
            long restaurantId = 25;
            int pageNumber = 2;
            int pageSize = 5;

            var expectedResult = new GetMenuResponseDto();

            _restaurantServiceMock
                .Setup(x => x.GetItemsAsync(
                    restaurantId ,
                    pageNumber ,
                    pageSize))
                .ReturnsAsync(expectedResult);

            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            await _controller.Menu(
                restaurantId ,
                pageNumber ,
                pageSize
            );

            _restaurantServiceMock.Verify(
                x => x.GetItemsAsync(
                    restaurantId ,
                    pageNumber ,
                    pageSize) ,
                Times.Once
            );
        }

    }
}
