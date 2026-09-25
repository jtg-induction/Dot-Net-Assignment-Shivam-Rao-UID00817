using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Razor.Parser;
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
        public async Task<List<BrowseRestaurantsResponseDto>> GetRestaurantsAsync(int pageNumber, CancellationToken cancellationToken = default)
        {
            List<Restaurants> activeRestaurants = await _restaurantRepository.GetActiveRestaurants(pageNumber);

            List<BrowseRestaurantsResponseDto> response = new List<BrowseRestaurantsResponseDto>();

            foreach (Restaurants restaurant in activeRestaurants) {
                response.Add(new BrowseRestaurantsResponseDto(
                    restaurant.RestaurantId, 
                    restaurant.Name, 
                    restaurant.AddressLine1, 
                    restaurant.City, 
                    restaurant.State, 
                    restaurant.Pincode, 
                    restaurant.Country, 
                    restaurant.AddressLine2));
            }
            return response;
        }

        /// <summary>
        /// Retrieves the list of all the available items for the requested restaurant.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="restaurant_id">The restaurant id of the restaurant whose menu is to be retrieved.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>List of all the available items for the restaurant.</returns>
        public async Task<BrowseMenuResponseDto> GetItemsAsync(int pageNumber, long restaurant_id, CancellationToken cancellationToken = default)
        {
            Restaurants restaurant = await _restaurantRepository.GetRestaurantByIdAsync(restaurant_id, false) ?? throw new ValidationException(ErrorMessages.RESTAURANT_DOES_NOT_EXIST);

            if(restaurant.IsActive == false)
            {
                throw new ValidationException(ErrorMessages.RESTAURANT_DOES_NOT_EXIST);
            }

            List<Items> activeItems = await _itemsRepository.GetItems(restaurant_id , pageNumber);

            List<ItemAndPrice> items = new List<ItemAndPrice>();

            foreach (Items item in activeItems)
            {
                items.Add(new ItemAndPrice(
                    item.ItemId ,
                    item.Name ,
                    item.Price
                    ));
            }

            BrowseMenuResponseDto response = new BrowseMenuResponseDto(new BrowseRestaurantsResponseDto(
                restaurant.RestaurantId, 
                restaurant.Name, 
                restaurant.City, 
                restaurant.State, 
                restaurant.Pincode, 
                restaurant.Country, 
                restaurant.AddressLine2), items);

            return response;
        }
    }
}
