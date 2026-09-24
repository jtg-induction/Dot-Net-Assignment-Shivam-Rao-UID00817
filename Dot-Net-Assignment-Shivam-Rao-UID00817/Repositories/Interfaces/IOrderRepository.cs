using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Orders CreateOrder(Orders order);
        void AddOrderItems(Order_Items orderItem);
        Task<List<Orders>> GetOrdersByUserId(long userId, int pageNumber, CancellationToken cancellationToken = default);
        Task<List<Orders>> GetOrdersByRestaurantId(long restaurantId, int pageNumber, string search, Enums.SortBy sortBy, Enums.FilterBy filterBy, string filterByCity = "", CancellationToken cancellationToken = default);
        Task<Orders> GetOrderById(long orderId , bool enableTracking, CancellationToken cancellationToken = default);
        Task<List<Order_Items>> GetOrderItems(long orderId, CancellationToken cancellationToken = default);
        Task<Orders> GetOrderWithUpdateLockAsync(long orderId, CancellationToken cancellationToken = default);
    }
}
