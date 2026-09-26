using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces
{
    public interface IRestaurantService
    {
        Task<List<GetRestaurantsResponseDto>> GetRestaurantsAsync(int pageNumber = 1, int pageSize = NumberConstants.PAGE_SIZE, CancellationToken cancellationToken = default);

        Task<GetMenuResponseDto> GetItemsAsync(long restaurant_id, int pageNumber = 1, int pageSize = NumberConstants.PAGE_SIZE, CancellationToken cancellationToken = default);
    }
}
