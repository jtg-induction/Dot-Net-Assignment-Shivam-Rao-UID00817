using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using System;
using System.Collections.Generic;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class GetCustomerOrdersDto
    {
        public List<OrderHistoryItems> Orders { get; set; } = new List<OrderHistoryItems>();
    }

    public class OrderHistoryItems
    {
        public long OrderId { get; set; }
        public string RestaurantName { get; set; }
        public decimal Amount { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdated { get; set; }

        public OrderHistoryItems(long orderId, string restaurantName, decimal amount, Enums.OrderStatus status, DateTime orderDate, DateTime lastUpdated)
        {
            this.OrderId = orderId;
            this.RestaurantName = restaurantName;
            this.Amount = amount;
            this.Status = status.ToString();
            this.OrderDate = orderDate;
            this.LastUpdated = lastUpdated;
        }

    }
}
