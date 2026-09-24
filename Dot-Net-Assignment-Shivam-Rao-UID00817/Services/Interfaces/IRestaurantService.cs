using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces
{
    public interface IRestaurantService
    {
        Task<List<BrowseRestaurantsResponseDto>> GetRestaurantsAsync(int pageNumber, CancellationToken cancellationToken = default);

        Task<BrowseMenuResponseDto> GetItemsAsync(int pageNumber, long restaurant_id, CancellationToken cancellationToken = default);
    }
}
