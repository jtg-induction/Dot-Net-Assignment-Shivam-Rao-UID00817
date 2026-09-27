using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IItemsRepository
    {
        IQueryable<Items> GetItems(long restaurantId);

        Task<List<Items>> GetItemsAsync(List<long> items, long restaurantId);

        Task<List<Items>> GetItemsWithUpdateLockAsync(List<long> itemIds, long restaurantId);
    }
}
