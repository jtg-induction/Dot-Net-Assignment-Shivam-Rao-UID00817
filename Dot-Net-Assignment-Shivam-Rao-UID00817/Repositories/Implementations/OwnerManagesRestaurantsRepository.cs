using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories
{
    public class OwnerManagesRestaurantsRepository : IOwnerManagesRestaurantsRepository
    {
        protected readonly Restaurant_ManagementContext _db;

        public OwnerManagesRestaurantsRepository(Restaurant_ManagementContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Adds multiple restaurant ownership entries to the database context.
        /// </summary>
        /// <param name="Owners">The ownership entries to add.</param>
        public void Add(List<Owner_Manages_Restaurants> Owners)
        {
            _db.Owner_Manages_Restaurants.AddRange(Owners);
        }


        /// <summary>
        /// Retrieves an ownership entry for a user and restaurant, with optional change tracking.
        /// </summary>
        /// <param name="userId">The ID of the owner.</param>
        /// <param name="restaurantId">The ID of the restaurant.</param>
        /// <param name="enableTracking">Whether to enable entity tracking.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The ownership entry if found; otherwise, null.</returns>
        public async Task<Owner_Manages_Restaurants> GetOwnerIfExistsAsync(long userId, long restaurantId, bool enableTracking = false, CancellationToken cancellationToken = default)
        {
            if (enableTracking)
            {
                return await _db.Owner_Manages_Restaurants.FindAsync(restaurantId, userId);
            }
            else
            {
                return await _db.Owner_Manages_Restaurants.AsNoTracking().FirstOrDefaultAsync(o => o.UserId == userId && o.RestaurantId == restaurantId);
            }
        }


        /// <summary>
        /// Removes a restaurant ownership entry from the database context.
        /// </summary>
        /// <param name="Entry">The ownership entry to remove.</param>
        public void Remove(Owner_Manages_Restaurants Entry)
        {
            _db.Owner_Manages_Restaurants.Remove(Entry);
        }
    }
}
