using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IItemsRepository
    {
        Task<List<Items>> GetItems(long restaurantId, int pageNumber, CancellationToken cancellationToken = default);

        Task<List<Items>> GetItemsAsync(List<long> items, long restaurantId, CancellationToken cancellationToken = default);

        Task<List<Items>> GetItemsWithUpdateLockAsync(List<long> itemIds, long restaurantId, CancellationToken cancellationToken = default);
    }
}
