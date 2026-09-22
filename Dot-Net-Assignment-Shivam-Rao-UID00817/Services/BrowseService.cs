using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using ValidationException = Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions.ValidationException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Razor.Parser;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services
{
    public class BrowseService : IBrowseService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IItemsRepository _itemsRepository;

        public BrowseService(IRestaurantRepository restaurantRepository, IItemsRepository itemsRepository)
        {
            _restaurantRepository = restaurantRepository;
            _itemsRepository = itemsRepository;
        }


        public async Task<List<BrowseRestaurantsResponseDto>> GetRestaurantsAsync()
        {
            List<Restaurants> activeRestaurants = await _restaurantRepository.GetActiveRestaurantsAsync();

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

        public async Task<BrowseMenuResponseDto> GetItemsAsync(long restaurant_id)
        {
            Restaurants restaurant = await _restaurantRepository.GetRestaurantByIdAsync(restaurant_id, false) ?? throw new ValidationException(ErrorMessages.RESTAURANT_DOESNOT_EXIST);

            if(restaurant.IsActive == false)
            {
                throw new ValidationException(ErrorMessages.RESTAURANT_DOESNOT_EXIST);
            }

            List<Items> activeItems = await _itemsRepository.GetItemsAsync(restaurant_id);

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
