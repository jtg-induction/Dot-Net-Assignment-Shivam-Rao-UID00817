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
        Task<List<Orders>> GetOrdersByUserId(long userId, PaginationMetadata meta, int pageNumber = 1, int pageSize = NumberConstants.PAGE_SIZE, CancellationToken cancellationToken = default);
        Task<List<RestaurantOrderHistoryItems>> GetOrdersByRestaurantId(long restaurantId, PaginationMetadata meta, string status = "", int pageNumber = 1, int pageSize = NumberConstants.PAGE_SIZE, string search = "", string sortBy = SortBy.UPDATE_DATE, Enums.SortDirection sortDirection = Enums.SortDirection.DESC, string City = "", CancellationToken cancellationToken = default);
        Task<Orders> GetOrderById(long orderId, bool enableTracking, CancellationToken cancellationToken = default);
        Task<List<Order_Items>> GetOrderItemsByOrderId(long orderId, CancellationToken cancellationToken = default);
        Task<Orders> GetOrderWithUpdateLock(long orderId, CancellationToken cancellationToken = default);
    }
}
