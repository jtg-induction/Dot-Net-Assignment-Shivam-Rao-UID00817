using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    [Table("Restaurants")]
    public class Restaurants : IAuditableEntity
    {
        [Key]
        [Column("restaurant_id")]
        public long RestaurantId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("address_line1")]
        public string AddressLine1 { get; set; }

        [Column("address_line2")]
        public string AddressLine2 { get; set; }

        [Required]
        [Column("city")]
        public string City { get; set; }

        [Required]
        [Column("state")]
        public string State { get; set; }

        [Required]
        [Column("pincode")]
        [MaxLength(12), MinLength(3)]
        public string Pincode { get; set; }

        [Required]
        [Column("country")]
        public string Country { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Menus> Menus { get; set; }
        public virtual ICollection<Owner_Manages_Restaurants> OwnerManagesRestaurants { get; set; }

        public Restaurants(string name, string addressLine1, string city, string state, string pincode, string country, string addressLine2 = "")
        {
            Name = name.Trim();
            AddressLine1 = addressLine1.Trim();
            AddressLine2 = addressLine2.Trim();
            City = city.Trim();
            State = state.Trim();
            Pincode = pincode.Trim();
            Country = country.Trim();
            DateTime currentTime = DateTime.UtcNow;
            CreatedAt = currentTime;
            UpdatedAt = currentTime;
            IsActive = true;
        }

        public Restaurants()
        {

        }
    }
}
