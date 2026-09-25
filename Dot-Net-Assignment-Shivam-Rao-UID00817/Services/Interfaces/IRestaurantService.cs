using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces
{
    public interface IRestaurantService
    {
        Task<List<GetRestaurantsResponseDto>> GetRestaurantsAsync(int pageNumber, CancellationToken cancellationToken = default);

        Task<GetMenuResponseDto> GetItemsAsync(int pageNumber, long restaurant_id, CancellationToken cancellationToken = default);
    }
}
