using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces
{
    public interface IRestaurantService
    {
        Task<List<BrowseRestaurantsResponseDto>> GetRestaurantsAsync(int pageNumber);

        Task<BrowseMenuResponseDto> GetItemsAsync(int pageNumber, long restaurant_id);
    }
}
