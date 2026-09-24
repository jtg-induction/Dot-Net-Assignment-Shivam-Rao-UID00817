using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string Country { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<RestaurantOrderItem> Items { get; set; } = new List<RestaurantOrderItem>();

        public GetRestaurantOrderDetailsDto(long orderId ,string customerName, string phoneNumber, string restaurantName , Enums.OrderStatus status , string instructions , decimal totalAmount , string addressLine1 , string city , string state , string pincode , string country , DateTime orderDate , DateTime updatedAt , string addressLine2 = "")
        {
            this.OrderId = orderId;
            this.CustomerName = customerName;
            this.PhoneNumber = phoneNumber;
            this.RestaurantName = restaurantName;
            this.Status = status.ToString();
            this.TotalAmount = totalAmount;
            this.Instructions = instructions;
            this.AddressLine1 = addressLine1;
            this.AddressLine2 = addressLine2;
            this.City = city;
            this.State = state;
            this.Pincode = pincode;
            this.Country = country;
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

        public RestaurantOrderItem(string itemName , decimal unitPrice , int quantity)
        {
            this.ItemName = itemName;
            this.UnitPrice = unitPrice;
            this.Quantity = quantity;
        }
    }
}
