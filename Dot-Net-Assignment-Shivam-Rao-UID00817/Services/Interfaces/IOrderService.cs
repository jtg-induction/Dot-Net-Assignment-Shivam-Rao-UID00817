using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDto> PlaceOrderAsync(long userId, OrderRequestDto order, CancellationToken cancellationToken = default);
        Task<GetCustomerOrdersDto> GetAllOrdersAsync(long userId, int pageNumber, CancellationToken cancellationToken = default);
        Task<GetRestaurantOrdersDto> GetAllOrdersAsync(long userId, long restaurantId, int pageNumber, string search, Enums.SortBy sortBy, Enums.FilterBy filterBy, string filterByCity = "", CancellationToken cancellationToken = default);
        Task<GetCustomerOrderDetailsDto> GetOrderDetailsAsync(long userId, long orderId, CancellationToken cancellationToken = default);
        Task<GetRestaurantOrderDetailsDto> GetOrderDetailsAsync(long userId, long restaurantId, long orderId, CancellationToken cancellationToken = default);
        Task CancelOrderAsync(long userId, long orderId, CancellationToken cancellationToken = default);
        Task ManageOrderAsync(long restaurantId, long ownerId, long orderId, OrderManagementRequestDto model, CancellationToken cancellationToken = default);
    }
}
