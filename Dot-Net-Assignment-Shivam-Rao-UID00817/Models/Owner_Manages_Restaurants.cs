using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    [Table("Owner_Manages_Restaurants")]
    public class Owner_Manages_Restaurants
    {
        [Key]
        [Column("restaurant_id", Order = 1)]
        public long RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public virtual Restaurants Restaurants { get; set; }

        [Key]
        [Column("user_id", Order = 2)]
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual Users Users { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
