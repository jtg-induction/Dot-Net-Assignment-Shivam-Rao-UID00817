using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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

        public IQueryable<Items> GetItems(long restaurantId)
        {
            return _db.Items.Where(x => (x.RestaurantId == restaurantId && x.IsActive && x.AvailableQuantity > 0));
        }

        public async Task<List<Items>> GetItemsAsync(List<long> itemIds, long restaurantId)
        {
            return await _db.Items.Where(x => (x.RestaurantId == restaurantId && x.IsActive && itemIds.Contains(x.ItemId))).ToListAsync();
        }

        public async Task<List<Items>> GetItemsWithUpdateLockAsync(List<long> itemIds, long restaurantId)
        {
            var sqlParams = new List<SqlParameter>();
            var restaurantParam = new SqlParameter("@RestaurantId" , restaurantId);
            sqlParams.Add(restaurantParam);

            var parameterNames = new List<string>();
            for (int i = 0; i < itemIds.Count; i++)
            {
                string paramName = $"@item{i}";
                parameterNames.Add(paramName);
                sqlParams.Add(new SqlParameter(paramName , itemIds[i]));
            }

            string inClause = string.Join(", " , parameterNames);

            string query = $@"
                            SELECT item_id AS ItemId, 
                            restaurant_id AS RestaurantId, 
                            name AS Name, 
                            price AS Price, 
                            available_quantity AS AvailableQuantity, 
                            created_at AS CreatedAt, 
                            is_active AS IsActive
                            FROM Items WITH (UPDLOCK, ROWLOCK) 
                            WHERE restaurant_id = @RestaurantId 
                            AND is_active = 1
                            AND item_id IN ({inClause});";

            return await _db.Items.SqlQuery(query , sqlParams.ToArray()).ToListAsync();
        }
    }
}
