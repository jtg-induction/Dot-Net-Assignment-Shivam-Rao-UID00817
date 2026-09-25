using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using ValidationException = Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions.ValidationException;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Tests.Services
{
    [TestFixture]
    public class RestaurantServiceTests
    {
        private Mock<IRestaurantRepository> _mockRestaurantRepository;
        private Mock<IItemRepository> _mockItemsRepository;

        private RestaurantService _restaurantService;

        [SetUp]
        public void Setup()
        {
            _mockRestaurantRepository = new Mock<IRestaurantRepository>();
            _mockItemsRepository = new Mock<IItemRepository>();

            _restaurantService = new RestaurantService(
                _mockRestaurantRepository.Object ,
                _mockItemsRepository.Object
            );
        }

        [Test]
        public async Task GetRestaurantsAsync_ActiveRestaurants_ReturnsMappedDtos()
        {
            int pageNumber = 1;

            var restaurants = new List<Restaurants>
        {
            new Restaurants
            {
                RestaurantId = 1,
                Name = "Pizza Palace",
                AddressLine1 = "123 Main Street",
                City = "Gurgaon",
                State = "Haryana",
                Pincode = "122001",
                Country = "India",
                AddressLine2 = "Near Mall"
            }
        };

            _mockRestaurantRepository
                .Setup(x => x.GetActiveRestaurants(pageNumber))
                .ReturnsAsync(restaurants);

            var result = await _restaurantService
                .GetRestaurantsAsync(pageNumber);

            Assert.That(result ,Is.Not.Null);
            Assert.That(result.Count ,Is.EqualTo(1));

            Assert.That(result[0].RestaurantId ,Is.EqualTo(1));
            Assert.That(result[0].RestaurantName ,Is.EqualTo("Pizza Palace"));
            Assert.That(result[0].City ,Is.EqualTo("Gurgaon"));

            _mockRestaurantRepository.Verify(
                x => x.GetActiveRestaurants(pageNumber) ,
                Times.Once
            );
        }

        [Test]
        public async Task GetRestaurantsAsync_NoRestaurants_ReturnsEmptyList()
        {
            int pageNumber = 1;

            _mockRestaurantRepository
                .Setup(x => x.GetActiveRestaurants(pageNumber))
                .ReturnsAsync(new List<Restaurants>());

            var result = await _restaurantService
                .GetRestaurantsAsync(pageNumber);

            Assert.That(result ,Is.Not.Null);
            Assert.That(result ,Is.Empty);
        }

        [Test]
        public async Task GetItemsAsync_ActiveRestaurant_ReturnsMenu()
        {
            long restaurantId = 1;
            int pageNumber = 1;

            var restaurant = new Restaurants
            {
                RestaurantId = restaurantId ,
                Name = "Pizza Palace" ,
                City = "Gurgaon" ,
                State = "Haryana" ,
                Pincode = "122001" ,
                Country = "India" ,
                AddressLine2 = "Near Mall" ,
                IsActive = true
            };

            var items = new List<Items>
        {
            new Items
            {
                ItemId = 101,
                Name = "Margherita Pizza",
                Price = 299
            }
        };

            _mockRestaurantRepository
                .Setup(x => x.GetRestaurantByIdAsync(restaurantId ,false))
                .ReturnsAsync(restaurant);

            _mockItemsRepository
                .Setup(x => x.GetItems(restaurantId ,pageNumber))
                .ReturnsAsync(items);

            var result = await _restaurantService
                .GetItemsAsync(pageNumber ,restaurantId);

            Assert.That(result ,Is.Not.Null);
            Assert.That(result.Restaurant.RestaurantId ,Is.EqualTo(restaurantId));
            Assert.That(result.Restaurant.RestaurantName ,Is.EqualTo("Pizza Palace"));

            Assert.That(result.items.Count ,Is.EqualTo(1));
            Assert.That(result.items[0].ItemId ,Is.EqualTo(101));
            Assert.That(result.items[0].Name ,Is.EqualTo("Margherita Pizza"));
            Assert.That(result.items[0].Price ,Is.EqualTo(299));

            _mockRestaurantRepository.Verify(
                x => x.GetRestaurantByIdAsync(restaurantId ,false) ,
                Times.Once
            );

            _mockItemsRepository.Verify(
                x => x.GetItems(restaurantId ,pageNumber) ,
                Times.Once
            );
        }

        [Test]
        public void GetItemsAsync_RestaurantDoesNotExist_ThrowsValidationException()
        {
            long restaurantId = 999;
            int pageNumber = 1;

            _mockRestaurantRepository
                .Setup(x => x.GetRestaurantByIdAsync(restaurantId ,false))
                .ReturnsAsync((Restaurants)null);

            var exception = Assert.ThrowsAsync<ValidationException>(
                async () =>
                    await _restaurantService
                        .GetItemsAsync(pageNumber ,restaurantId)
            );

            Assert.That(
                exception.Message ,
                Is.EqualTo(ErrorMessages.RESTAURANT_DOES_NOT_EXIST)
            );

            _mockItemsRepository.Verify(
                x => x.GetItems(It.IsAny<long>() ,It.IsAny<int>()) ,
                Times.Never
            );
        }

        [Test]
        public void GetItemsAsync_InactiveRestaurant_ThrowsValidationException()
        {
            long restaurantId = 1;
            int pageNumber = 1;

            var restaurant = new Restaurants
            {
                RestaurantId = restaurantId ,
                IsActive = false
            };

            _mockRestaurantRepository
                .Setup(x => x.GetRestaurantByIdAsync(restaurantId ,false))
                .ReturnsAsync(restaurant);

            var exception = Assert.ThrowsAsync<ValidationException>(
                async () =>
                    await _restaurantService
                        .GetItemsAsync(pageNumber ,restaurantId)
            );

            Assert.That(
                exception.Message ,
                Is.EqualTo(ErrorMessages.RESTAURANT_DOES_NOT_EXIST)
            );

            _mockItemsRepository.Verify(
                x => x.GetItems(It.IsAny<long>() ,It.IsAny<int>()) ,
                Times.Never
            );
        }
    }
}
