using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    [Table("Restaurants")]
    public class Restaurants
    {
        [Key]
        [Column("restaurant_id")]
        public long RestaurantId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("address_id")]
        public long AddressId { get; set; }

        [ForeignKey(nameof(AddressId))]
        public virtual Addresses Addresses { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Menus> Menus { get; set; }
        public virtual ICollection<Owner_Manages_Restaurants> OwnerManagesRestaurants { get; set; }
    }
}
