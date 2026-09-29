using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ValidationException = Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions.ValidationException;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IItemRepository _itemsRepository;

        public RestaurantService(IRestaurantRepository restaurantRepository, IItemRepository itemsRepository)
        {
            _restaurantRepository = restaurantRepository;
            _itemsRepository = itemsRepository;
        }


        /// <summary>
        /// Retireves the list of the available restaurants.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>A list of all the active restaurants.</returns>
        public async Task<RestaurantsList> GetRestaurantsAsync(int pageNumber = 1, int pageSize = NumberConstants.PAGE_SIZE, CancellationToken cancellationToken = default)
        {
            var meta = new PaginationMetadata();
            List<Restaurants> activeRestaurants = await _restaurantRepository.GetActiveRestaurants(meta, pageNumber, pageSize, cancellationToken);

            List<GetRestaurantsResponseDto> data = new List<GetRestaurantsResponseDto>();

            foreach (Restaurants restaurant in activeRestaurants)
            {
                data.Add(new GetRestaurantsResponseDto(
                    restaurant.RestaurantId,
                    restaurant.Name,
                    restaurant.AddressLine1,
                    restaurant.City,
                    restaurant.State,
                    restaurant.Pincode,
                    restaurant.Country,
                    restaurant.AddressLine2));
            }

            var response = new RestaurantsList { Restaurants = data, Meta = meta };

            return response;
        }

        /// <summary>
        /// Retrieves the list of all the available items for the requested restaurant.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="restaurant_id">The restaurant id of the restaurant whose menu is to be retrieved.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>List of all the available items for the restaurant.</returns>
        public async Task<GetMenuResponseDto> GetItemsAsync(long restaurant_id, int pageNumber = 1, int pageSize = NumberConstants.PAGE_SIZE, CancellationToken cancellationToken = default)
        {
            Restaurants restaurant = await _restaurantRepository.GetRestaurantById(restaurant_id, false, cancellationToken) ?? throw new ValidationException(ErrorMessages.RESTAURANT_DOES_NOT_EXIST);

            if (!restaurant.IsActive)
            {
                throw new ValidationException(ErrorMessages.RESTAURANT_DOES_NOT_EXIST);
            }

            var meta = new PaginationMetadata();

            List<Items> activeItems = await _itemsRepository.GetAllItemsByRestaurantId(restaurant_id, meta, pageNumber, pageSize, false, cancellationToken);

            List<ItemAndPrice> items = new List<ItemAndPrice>();

            foreach (Items item in activeItems)
            {
                items.Add(new ItemAndPrice(
                    item.ItemId,
                    item.Name,
                    item.Price
                    ));
            }

            GetMenuResponseDto response = new GetMenuResponseDto(new GetRestaurantsResponseDto(
                restaurant.RestaurantId,
                restaurant.Name,
                restaurant.City,
                restaurant.State,
                restaurant.Pincode,
                restaurant.Country,
                restaurant.AddressLine2), items, meta);

            return response;
        }
    }
}
