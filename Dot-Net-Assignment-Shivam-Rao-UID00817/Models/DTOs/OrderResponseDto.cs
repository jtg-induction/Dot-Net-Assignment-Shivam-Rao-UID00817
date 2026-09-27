using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class OrderResponseDto
    {
        public long OrderId { get; set; }

        public long UserId { get; set; }

        public long RestaurantId { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; }

        public List<OrderItemResponseDto> Items { get; set; }
    }

    public class OrderItemResponseDto
    {
        public long ItemId { get; set; }

        public string Name { get; set; }

        public decimal ItemPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
