using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IRestaurantRepository
    {
        void Add(Restaurants restaurant);
        Task<Restaurants> GetRestaurantByName(string restaurantName, bool enableTracking, CancellationToken cancellationToken = default);
        Task<Restaurants> GetRestaurantById(long restaurantId, bool enableTracking, CancellationToken cancellationToken = default);
        Task<List<Restaurants>> GetActiveRestaurants(int pageNumber = 1, int pageSize = NumberConstants.PAGE_SIZE, CancellationToken cancellationToken = default);
    }
}
