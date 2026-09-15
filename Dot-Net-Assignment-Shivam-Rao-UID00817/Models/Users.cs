using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        [Column("user_id")]
        public long UserId { get; set; }

        [Required]
        [Index("IX_User_Email", IsUnique = true)]
        [StringLength(255)]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Index("IX_User_PhoneNumber", IsUnique = true)]
        [StringLength(50)]
        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("role")]
        public string Role { get; set; }

        [Column("wallet_balance")]
        public decimal WalletBalance { get; set; } = 1000.0m;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<User_Address_Type> UserAddressTypes { get; set; }
        public virtual ICollection<Owner_Manages_Restaurants> OwnerManagesRestaurants { get; set; }
    }
}
