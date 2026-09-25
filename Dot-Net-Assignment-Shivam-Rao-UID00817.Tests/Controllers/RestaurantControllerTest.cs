using Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;


namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Tests.Controllers
{
    [TestFixture]
    public class RestaurantControllerTest
    {
        private Mock<IRestaurantService> _mockRestaurantService;

        private RestaurantController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRestaurantService = new Mock<IRestaurantService>();

            _controller = new RestaurantController(
                _mockRestaurantService.Object
            );

            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();
        }

        [Test]
        public async Task Restaurant_ValidRequest_ReturnsOk()
        {

            int pageNumber = 1;

            var expectedResult = new List<GetRestaurantsResponseDto>
        {
            new GetRestaurantsResponseDto(
                1,
                "Pizza Palace",
                "123 Main Street",
                "Gurgaon",
                "Haryana",
                "122001",
                "India",
                "Near Mall"
            )
        };

            _mockRestaurantService
                .Setup(x => x.GetRestaurantsAsync(
                    pageNumber ,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);


            HttpResponseMessage response =
                await _controller.Restaurant(pageNumber);


            Assert.That(response ,Is.Not.Null);
            Assert.That(
                response.StatusCode ,
                Is.EqualTo(HttpStatusCode.OK)
            );

            var result =
                await response.Content
                    .ReadAsAsync<List<GetRestaurantsResponseDto>>();

            Assert.That(result ,Is.Not.Null);
            Assert.That(result.Count ,Is.EqualTo(1));

            Assert.That(result[0].RestaurantId ,Is.EqualTo(1));
            Assert.That(result[0].RestaurantName ,Is.EqualTo("Pizza Palace"));

            _mockRestaurantService.Verify(
                x => x.GetRestaurantsAsync(
                    pageNumber ,
                    It.IsAny<CancellationToken>()) ,
                Times.Once
            );
        }

        [Test]
        public async Task Restaurant_DefaultPageNumber_CallsServiceWithPageOne()
        {

            var expectedResult =
                new List<GetRestaurantsResponseDto>();

            _mockRestaurantService
                .Setup(x => x.GetRestaurantsAsync(
                    1 ,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);


            var response = await _controller.Restaurant();


            Assert.That(
                response.StatusCode ,
                Is.EqualTo(HttpStatusCode.OK)
            );

            _mockRestaurantService.Verify(
                x => x.GetRestaurantsAsync(
                    1 ,
                    It.IsAny<CancellationToken>()) ,
                Times.Once
            );
        }

        [Test]
        public async Task Menu_ValidRequest_ReturnsOk()
        {

            long restaurantId = 1;
            int pageNumber = 1;

            var expectedResult = new GetMenuResponseDto(
                new GetRestaurantsResponseDto(
                    1 ,
                    "Pizza Palace" ,
                    "Gurgaon" ,
                    "Haryana" ,
                    "122001" ,
                    "India" ,
                    "Near Mall"
                ) ,
                new List<ItemAndPrice>
                {
            new ItemAndPrice(
                101,
                "Margherita Pizza",
                299
            )
                }
            );

            _mockRestaurantService
                .Setup(x => x.GetItemsAsync(
                    pageNumber ,
                    restaurantId ,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);


            HttpResponseMessage response =
                await _controller.Menu(
                    restaurantId ,
                    pageNumber
                );


            Assert.That(response ,Is.Not.Null);

            Assert.That(
                response.StatusCode ,
                Is.EqualTo(HttpStatusCode.OK)
            );

            var result =
                await response.Content
                    .ReadAsAsync<GetMenuResponseDto>();

            Assert.That(result ,Is.Not.Null);

            Assert.That(
                result.Restaurant.RestaurantId ,
                Is.EqualTo(restaurantId)
            );

            Assert.That(
                result.Restaurant.RestaurantName ,
                Is.EqualTo("Pizza Palace")
            );

            Assert.That(
                result.items.Count ,
                Is.EqualTo(1)
            );

            Assert.That(
                result.items[0].ItemId ,
                Is.EqualTo(101)
            );

            Assert.That(
                result.items[0].Name ,
                Is.EqualTo("Margherita Pizza")
            );

            Assert.That(
                result.items[0].Price ,
                Is.EqualTo(299)
            );

            _mockRestaurantService.Verify(
                x => x.GetItemsAsync(
                    pageNumber ,
                    restaurantId ,
                    It.IsAny<CancellationToken>()) ,
                Times.Once
            );
        }

        [Test]
        public async Task Menu_DefaultPageNumber_CallsServiceWithPageOne()
        {

            long restaurantId = 1;

            var expectedResult = new GetMenuResponseDto(
                new GetRestaurantsResponseDto(
                    1 ,
                    "Pizza Palace" ,
                    "Gurgaon" ,
                    "Haryana" ,
                    "122001" ,
                    "India" ,
                    "Near Mall"
                ) ,
                new List<ItemAndPrice>()
            );

            _mockRestaurantService
                .Setup(x => x.GetItemsAsync(
                    1 ,
                    restaurantId ,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);


            var response = await _controller.Menu(restaurantId);


            Assert.That(
                response.StatusCode ,
                Is.EqualTo(HttpStatusCode.OK)
            );

            _mockRestaurantService.Verify(
                x => x.GetItemsAsync(
                    1 ,
                    restaurantId ,
                    It.IsAny<CancellationToken>()) ,
                Times.Once
            );
        }
    }
}
