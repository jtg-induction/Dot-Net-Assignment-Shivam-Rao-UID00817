using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Orders CreateOrder(Orders order);
        void AddOrderItems(Order_Items orderItem);
        Task<List<Orders>> GetOrdersByUserId(long userId, int pageNumber = 1, int pageSize = NumberConstants.PAGE_SIZE, CancellationToken cancellationToken = default);
        Task<List<Orders>> GetOrdersByRestaurantId(long restaurantId, PaginationMetadata meta, Enums.FilterBy filterBy, int pageNumber = 1, int pageSize = NumberConstants.PAGE_SIZE, string search = "", Enums.SortBy sortBy = Enums.SortBy.OrderDateLatest, string filterByCity = "", CancellationToken cancellationToken = default);
        Task<Orders> GetOrderById(long orderId, bool enableTracking, CancellationToken cancellationToken = default);
        Task<List<Order_Items>> GetOrderItemsByOrderId(long orderId, CancellationToken cancellationToken = default);
        Task<Orders> GetOrderWithUpdateLock(long orderId, CancellationToken cancellationToken = default);
    }
}
