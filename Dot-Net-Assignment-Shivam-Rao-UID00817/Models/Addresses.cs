using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    [Table("Addresses")]
    public class Addresses
    {
        [Key]
        [Column("address_id")]
        public long AddressId { get; set; }

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
        [MaxLength(6), MinLength(6)]
        public string Pincode { get; set; }

        [Required]
        [Column("country")]
        public string Country { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Restaurants> Restaurants { get; set; }
        public virtual ICollection<User_Address_Type> UserAddressTypes { get; set; }
    }
}
