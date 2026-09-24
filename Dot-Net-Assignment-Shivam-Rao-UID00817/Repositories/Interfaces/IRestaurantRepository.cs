using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IRestaurantRepository
    {
        void Add(Restaurants restaurant);
        Task<Restaurants> GetRestaurantAsync(string restaurantName, bool enableTracking, CancellationToken cancellationToken = default);
        Task<Restaurants> GetRestaurantByIdAsync(long restaurantId, bool enableTracking, CancellationToken cancellationToken = default);
        Task<List<Restaurants>> GetActiveRestaurants(int pageNumber, CancellationToken cancellationToken = default);
    }
}
