using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IItemRepository
    {
        Task<List<Items>> GetAllItemsByRestaurantId(long restaurantId, PaginationMetadata meta,int pageNumber = 1, int pageSize = NumberConstants.PAGE_SIZE, bool includeInactive = false, CancellationToken cancellationToken = default);

        Task<List<Items>> GetValidItems(List<long> items, long restaurantId, bool includeInactive = false, CancellationToken cancellationToken = default);

        Task<List<Items>> GetItemsWithUpdateLock(List<long> itemIds, long restaurantId, CancellationToken cancellationToken = default);
    }
}
