using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IItemRepository
    {
        Task<List<Items>> GetItems(long restaurantId, int pageNumber, bool includeInactive = false, CancellationToken cancellationToken = default);

        Task<List<Items>> GetItemsAsync(List<long> items, long restaurantId, bool includeInactive = false, CancellationToken cancellationToken = default);

        Task<List<Items>> GetItemsWithUpdateLockAsync(List<long> itemIds, long restaurantId, CancellationToken cancellationToken = default);
    }
}
