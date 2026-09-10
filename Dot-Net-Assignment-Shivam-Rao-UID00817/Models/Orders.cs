using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    [Table("Orders")]
    public class Orders
    {
        [Key]
        [Column("order_id")]
        public long OrderId { get; set; }

        [Column("instructions")]
        public string Instructions { get; set; }

        [Required]
        [Column("status")]
        public string Status { get; set; }

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

        [Column("user_id")]
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual Users Users { get; set; }

        public virtual ICollection<Order_Items> OrderItems { get; set; }
    }
}
