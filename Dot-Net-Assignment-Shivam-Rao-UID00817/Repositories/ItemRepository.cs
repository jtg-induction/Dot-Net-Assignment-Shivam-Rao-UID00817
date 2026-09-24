using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
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

        /// <summary>
        /// Retrieves active and available items for a restaurant using pagination.
        /// </summary>
        /// <param name="restaurantId">The ID of the restaurant.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>A list of available items.</returns>
        public async Task<List<Items>> GetItems(long restaurantId, int pageNumber = 1, CancellationToken cancellationToken = default)
        {
            return await _db.Items.Where(x => (x.RestaurantId == restaurantId && x.IsActive && x.AvailableQuantity > 0)).OrderBy(x => x.RestaurantId)
                                                                                                                        .Skip((pageNumber - 1) * NumberConstants.PAGE_SIZE)
                                                                                                                        .Take(NumberConstants.PAGE_SIZE)
                                                                                                                        .ToListAsync(); ;
        }

        /// <summary>
        /// Retrieves active items matching the specified IDs for a restaurant.
        /// </summary>
        /// <param name="itemIds">The IDs of the items to retrieve.</param>
        /// <param name="restaurantId">The ID of the restaurant.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>A list of matching items.</returns>
        public async Task<List<Items>> GetItemsAsync(List<long> itemIds, long restaurantId, CancellationToken cancellationToken = default)
        {
            return await _db.Items.Where(x => (x.RestaurantId == restaurantId && x.IsActive && itemIds.Contains(x.ItemId))).ToListAsync();
        }

        /// <summary>
        /// Retrieves active items by ID while applying update and row-level locks.
        /// </summary>
        /// <param name="itemIds">The IDs of the items to retrieve and lock.</param>
        /// <param name="restaurantId">The ID of the restaurant.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>A list of matching locked items.</returns>
        public async Task<List<Items>> GetItemsWithUpdateLockAsync(List<long> itemIds, long restaurantId, CancellationToken cancellationToken = default)
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
