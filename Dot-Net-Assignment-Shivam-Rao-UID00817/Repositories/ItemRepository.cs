using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories
{
    public class ItemRepository : IItemsRepository
    {
        private readonly Restaurant_ManagementContext _db;

        public ItemRepository(Restaurant_ManagementContext db)
        {
            _db = db;
        }

        public async Task<List<Items>> GetItemsAsync(long restaurantId)
        {
            return await _db.Items.Where(x => (x.RestaurantId == restaurantId && x.IsActive && x.AvailableQuantity > 0)).ToListAsync();
        }

        public async Task<List<Items>> GetItemsAsync(List<long> itemIds, long restaurantId)
        {
            return await _db.Items.Where(x => (x.RestaurantId == restaurantId && x.IsActive && itemIds.Contains(x.ItemId))).ToListAsync();
        }

        public async Task<int> DecreaseQuantityIfAvailableAsync(long itemId, long restaurantId, int quantity)
        {
            return await _db.Database.ExecuteSqlCommandAsync(
                @"UPDATE items
                    SET available_quantity = available_quantity - @p0
                    WHERE item_id = @p1
                        AND restaurant_id = @p2
                        AND is_active = 1
                        AND available_quantity >= @p0
                " , quantity , itemId , restaurantId);
        }
    }
}
