using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    [Table("Addresses")]
    public class Addresses : IAuditableEntity
    {
        [Key]
        [Column("address_id")]
        public long AddressId { get; set; }

        [Index("IX_User_Id")]
        [Column("user_id")]
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual Users Users { get; set; }

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

        public Addresses(string addressLine1, string addressLine2, string city, string state, string pincode, string country)
        {
            DateTime currentTime = DateTime.UtcNow;
            this.AddressLine1 = addressLine1;
            this.AddressLine2 = addressLine2;
            this.City = city;
            this.State = state;
            this.Pincode = pincode;
            this.Country = country;
            this.CreatedAt = currentTime;
            this.UpdatedAt = currentTime;
        }

        public Addresses()
        {

        }
    }


}
