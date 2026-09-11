using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    [Table("Menus")]
    public class Menus
    {
        [Key]
        [Column("menu_id")]
        public long MenuId { get; set; }

        [Required]
        [Column("restaurant_id")]
        public long RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public virtual Restaurants Restaurants { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Menu_Items> MenuItems { get; set; }
    }
}
