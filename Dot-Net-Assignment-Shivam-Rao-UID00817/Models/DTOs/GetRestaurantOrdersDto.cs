using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class GetRestaurantOrdersDto
    {
        public List<RestaurantOrderHistoryItems> Orders { get; set; } = new List<RestaurantOrderHistoryItems>();
    }
    public class RestaurantOrderHistoryItems
    {
        public long OrderId { get; set; }
        public string RestaurantName { get; set; }
        public decimal Amount { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdated { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }

        public int ItemCount { get; set; }

        public RestaurantOrderHistoryItems(long orderId , string restaurantName , decimal amount , Enums.OrderStatus status , DateTime orderDate , DateTime lastUpdated, string addressLine1, string city, int itemCount)
        {
            this.OrderId = orderId;
            this.RestaurantName = restaurantName;
            this.Amount = amount;
            this.Status = status.ToString();
            this.OrderDate = orderDate;
            this.LastUpdated = lastUpdated;
            this.AddressLine1 = addressLine1;
            this.City = city;
            this.ItemCount = itemCount;
        }
    }
}
