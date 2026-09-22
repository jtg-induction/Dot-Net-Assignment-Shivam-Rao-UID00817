using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    [Table("Items")]
    public class Items
    {
        [Key]
        [Column("item_id")]
        public long ItemId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Index("IX_Restaurant_Id")]
        [Column("restaurant_id")]
        public long RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public virtual Restaurants Restaurants { get; set; }

        [Required]
        [Column("price")]
        public decimal Price { get; set; }

        [Column("available_quantity")]
        public int AvailableQuantity { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Order_Items> OrderItems { get; set; }
    }
}
