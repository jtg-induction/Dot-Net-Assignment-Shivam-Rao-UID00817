using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories
{
    public class OwnerManagesRestaurantsRepository : IOwnerManagesRestaurantsRepository
    {
        protected readonly Restaurant_ManagementContext _db;

        public OwnerManagesRestaurantsRepository(Restaurant_ManagementContext db)
        {
            _db = db;
        }
        public void Add(List<Owner_Manages_Restaurants> Owners)
        {
            _db.Owner_Manages_Restaurants.AddRange(Owners);
        }

        public async Task<Owner_Manages_Restaurants> GetOwnerIfExistsAsync(long userId, long restaurantId, bool enableTracking = false)
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

        public void Remove(Owner_Manages_Restaurants Entry)
        {
            _db.Owner_Manages_Restaurants.Remove(Entry);
        }
    }
}
