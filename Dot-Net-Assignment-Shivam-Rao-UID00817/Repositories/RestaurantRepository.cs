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
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly Restaurant_ManagementContext _db;

        public RestaurantRepository(Restaurant_ManagementContext db)
        {
            _db = db;
        }

        public void Add(Restaurants restaurant)
        {
            _db.Restaurants.Add(restaurant);
        }

        public async Task<Restaurants> GetRestaurantAsync(string restaurantName , bool enableTracking)
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
    }
}
