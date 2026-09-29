using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using System;
using System.Collections.Generic;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class GetRestaurantOrderDetailsDto
    {
        public long OrderId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string RestaurantName { get; set; }
        public string Status { get; set; }
        public string Instructions { get; set; }
        public decimal TotalAmount { get; set; }
        public Address Address { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<RestaurantOrderItem> Items { get; set; } = new List<RestaurantOrderItem>();

        public GetRestaurantOrderDetailsDto(long orderId, string customerName, string phoneNumber, string restaurantName, Enums.OrderStatus status, string instructions, decimal totalAmount, DateTime orderDate, DateTime updatedAt)
        {
            this.OrderId = orderId;
            this.CustomerName = customerName;
            this.PhoneNumber = phoneNumber;
            this.RestaurantName = restaurantName;
            this.Status = status.ToString();
            this.TotalAmount = totalAmount;
            this.Instructions = instructions;
            this.OrderDate = orderDate;
            this.UpdatedAt = updatedAt;
        }

        public GetRestaurantOrderDetailsDto()
        {

        }

    }
    public class RestaurantOrderItem
    {
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public RestaurantOrderItem(string itemName, decimal unitPrice, int quantity)
        {
            this.ItemName = itemName;
            this.UnitPrice = unitPrice;
            this.Quantity = quantity;
        }
    }
}
