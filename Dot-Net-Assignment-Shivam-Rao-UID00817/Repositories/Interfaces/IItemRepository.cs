using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IItemRepository
    {
        Task<List<Items>> GetAllItemsByRestaurantId(long restaurantId, int pageNumber, int pageSize = NumberConstants.PAGE_SIZE, bool includeInactive = false, CancellationToken cancellationToken = default);

        Task<List<Items>> GetValidItems(List<long> items, long restaurantId, bool includeInactive = false, CancellationToken cancellationToken = default);

        Task<List<Items>> GetItemsWithUpdateLock(List<long> itemIds, long restaurantId, CancellationToken cancellationToken = default);
    }
}
