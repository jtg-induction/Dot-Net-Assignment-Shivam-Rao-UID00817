using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDto> PlaceOrderAsync(long userId, OrderRequestDto order);
        Task<GetOrdersDto> GetAllOrdersAsync(long userId, int pageNumber);
        Task<GetOrdersDto> GetAllOrdersAsync(long userId, long restaurantId, int pageNumber);
        Task<GetOrderDetailsDto> GetOrderDetailsAsync(long userId , long orderId);
        Task<GetOrderDetailsDto> GetOrderDetailsAsync(long userId , long restaurantId, long orderId);
    }
}
