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
using System.Data.Entity;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IItemsRepository _itemsRepository;

        public RestaurantService(IRestaurantRepository restaurantRepository, IItemsRepository itemsRepository)
        {
            _restaurantRepository = restaurantRepository;
            _itemsRepository = itemsRepository;
        }


        public async Task<List<BrowseRestaurantsResponseDto>> GetRestaurantsAsync(int pageNumber)
        {
            List<Restaurants> activeRestaurants = await _restaurantRepository.GetActiveRestaurants().OrderBy(x => x.RestaurantId)
                                                                                               .Skip((pageNumber - 1) * NumberConstants.PAGE_SIZE)
                                                                                               .Take(NumberConstants.PAGE_SIZE)
                                                                                               .ToListAsync();

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

        public async Task<BrowseMenuResponseDto> GetItemsAsync(int pageNumber, long restaurant_id)
        {
            Restaurants restaurant = await _restaurantRepository.GetRestaurantByIdAsync(restaurant_id, false) ?? throw new ValidationException(ErrorMessages.RESTAURANT_DOES_NOT_EXIST);

            if(restaurant.IsActive == false)
            {
                throw new ValidationException(ErrorMessages.RESTAURANT_DOES_NOT_EXIST);
            }

            List<Items> activeItems = await _itemsRepository.GetItems(restaurant_id).OrderBy(x => x.ItemId)
                                                                                .Skip((pageNumber - 1) * NumberConstants.PAGE_SIZE)
                                                                                .Take(NumberConstants.PAGE_SIZE)
                                                                                .ToListAsync();

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
