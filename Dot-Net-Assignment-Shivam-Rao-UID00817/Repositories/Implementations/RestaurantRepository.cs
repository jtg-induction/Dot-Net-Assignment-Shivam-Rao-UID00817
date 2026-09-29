using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using System;

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
        public async Task<List<Restaurants>> GetActiveRestaurants(PaginationMetadata meta, int pageNumber = 1, int pageSize = NumberConstants.PAGE_SIZE, CancellationToken cancellationToken = default)
        {
            var data = _db.Restaurants.Where(r => r.IsActive).OrderBy(x => x.RestaurantId);
            meta.TotalCount = data.Count();
            meta.CurrentPage = pageNumber;
            meta.PageSize = pageSize;
            return await data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a restaurant by name, with optional change tracking.
        /// </summary>
        /// <param name="restaurantName">The name of the restaurant.</param>
        /// <param name="enableTracking">Whether to enable entity tracking.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The restaurant if found; otherwise, null.</returns>
        public async Task<Restaurants> GetRestaurantByName(string restaurantName, bool enableTracking, CancellationToken cancellationToken = default)
        {
            if (enableTracking)
            {
                return await _db.Restaurants.FirstOrDefaultAsync(r => r.Name.ToLower() == restaurantName.ToLower(), cancellationToken);
            }
            else
            {
                return await _db.Restaurants.AsNoTracking().FirstOrDefaultAsync(r => r.Name.ToLower() == restaurantName.ToLower(), cancellationToken);
            }
        }

        /// <summary>
        /// Retrieves a restaurant by its ID, with optional change tracking.
        /// </summary>
        /// <param name="restaurantId">The ID of the restaurant.</param>
        /// <param name="enableTracking">Whether to enable entity tracking.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The restaurant if found; otherwise, null.</returns>
        public async Task<Restaurants> GetRestaurantById(long restaurantId, bool enableTracking, CancellationToken cancellationToken = default)
        {
            if (enableTracking)
            {
                return await _db.Restaurants.FindAsync(restaurantId, cancellationToken);
            }
            else
            {
                return await _db.Restaurants.AsNoTracking().FirstOrDefaultAsync(r => r.RestaurantId == restaurantId, cancellationToken);
            }
        }
    }
}
