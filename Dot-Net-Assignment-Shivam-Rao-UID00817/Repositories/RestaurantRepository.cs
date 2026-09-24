using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly Restaurant_ManagementContext _db;

        public RestaurantRepository(Restaurant_ManagementContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Adds a restaurant to the database context.
        /// </summary>
        /// <param name="restaurant">The restaurant to add.</param>
        public void Add(Restaurants restaurant)
        {
            _db.Restaurants.Add(restaurant);
        }

        /// <summary>
        /// Retrieves a paginated list of active restaurants.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>A list of active restaurants.</returns>
        public async Task<List<Restaurants>> GetActiveRestaurants(int pageNumber, CancellationToken cancellationToken = default)
        {
            return await _db.Restaurants.Where(r => r.IsActive).OrderBy(x => x.RestaurantId)
                                                                .Skip((pageNumber - 1) * NumberConstants.PAGE_SIZE)
                                                                .Take(NumberConstants.PAGE_SIZE)
                                                                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a restaurant by name, with optional change tracking.
        /// </summary>
        /// <param name="restaurantName">The name of the restaurant.</param>
        /// <param name="enableTracking">Whether to enable entity tracking.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The restaurant if found; otherwise, null.</returns>
        public async Task<Restaurants> GetRestaurantAsync(string restaurantName , bool enableTracking, CancellationToken cancellationToken = default)
        {
            if (enableTracking)
            {
                return await _db.Restaurants.FirstOrDefaultAsync(r => r.Name.ToLower() == restaurantName.ToLower());
            }
            else
            {
                return await _db.Restaurants.AsNoTracking().FirstOrDefaultAsync(r => r.Name.ToLower() == restaurantName.ToLower());
            }
        }

        /// <summary>
        /// Retrieves a restaurant by its ID, with optional change tracking.
        /// </summary>
        /// <param name="restaurantId">The ID of the restaurant.</param>
        /// <param name="enableTracking">Whether to enable entity tracking.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The restaurant if found; otherwise, null.</returns>
        public async Task<Restaurants> GetRestaurantByIdAsync(long restaurantId , bool enableTracking, CancellationToken cancellationToken = default)
        {
            if (enableTracking)
            {
                return await _db.Restaurants.FindAsync(restaurantId);
            }
            else
            {
                return await _db.Restaurants.AsNoTracking().FirstOrDefaultAsync(r => r.RestaurantId == restaurantId);
            }
        }
    }
}
