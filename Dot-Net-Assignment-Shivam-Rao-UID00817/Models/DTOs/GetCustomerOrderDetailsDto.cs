using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using System;
using System.Collections.Generic;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class GetCustomerOrderDetailsDto
    {
        public long OrderId { get; set; }
        public string RestaurantName { get; set; }
        public string Status { get; set; }
        public string Instructions { get; set; }
        public decimal TotalAmount { get; set; }
        public Address Address { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public GetCustomerOrderDetailsDto(long orderId, string restaurantName, Enums.OrderStatus status, string instructions, decimal totalAmount, DateTime orderDate, DateTime updatedAt)
        {
            this.OrderId = orderId;
            this.RestaurantName = restaurantName;
            this.Status = status.ToString();
            this.TotalAmount = totalAmount;
            this.Instructions = instructions;
            this.OrderDate = orderDate;
            this.UpdatedAt = updatedAt;
        }

        public GetCustomerOrderDetailsDto()
        {

        }

    }

    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string Country { get; set; }
    }
    public class OrderItem
    {
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public OrderItem(string itemName, decimal unitPrice, int quantity)
        {
            this.ItemName = itemName;
            this.UnitPrice = unitPrice;
            this.Quantity = quantity;
        }
    }
}
