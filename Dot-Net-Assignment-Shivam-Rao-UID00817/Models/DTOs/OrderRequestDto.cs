using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class OrderRequestDto
    {
        [Required]
        public long RestaurantId { get; set; }

        [Required]
        public long AddressId { get; set; }

        [Required]
        public List<OrderItemsReq> Items { get; set; } = new List<OrderItemsReq>();

        public string Instructions { get; set; }
    }

    public class OrderItemsReq
    {
        [Required]
        public long ItemId { get; set; }
        [Required,Range(1,int.MaxValue)]
        public int Quantity { get; set; }
    }
}
