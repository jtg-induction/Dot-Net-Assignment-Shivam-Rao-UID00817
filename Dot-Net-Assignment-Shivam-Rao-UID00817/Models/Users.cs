using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    [Table("Users")]
    public class Users : IAuditableEntity
    {
        [Key]
        [Column("user_id")]
        public long UserId { get; set; }

        [Required]
        [Index("IX_User_Email" , IsUnique = true)]
        [StringLength(255)]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Index("IX_User_PhoneNumber" , IsUnique = true)]
        [StringLength(50)]
        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("wallet_balance")]
        public decimal WalletBalance { get; set; } = NumberConstants.DEFAULT_ACCOUNT_BALANCE;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("role")]
        public Constants.Enums.Roles Role { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<Owner_Manages_Restaurants> OwnerManagesRestaurants { get; set; }
        public virtual ICollection<Refresh_Tokens> RefreshTokens { get; set; }
        public virtual ICollection<Addresses> Addresss { get; set; }
        public Users(string email , string phoneNumber , string password , string name)
        {
            DateTime CurrentTime = DateTime.UtcNow;
            this.Email = email;
            this.Password = HashingHelper.HashPassword(password.Trim());
            this.PhoneNumber = phoneNumber;
            this.Name = name;
            this.CreatedAt = CurrentTime;
            this.UpdatedAt = CurrentTime;
            this.WalletBalance = NumberConstants.DEFAULT_ACCOUNT_BALANCE;
            this.IsActive = true;
            this.Role = Constants.Enums.Roles.Customer;
        }
        public Users()
        {

        }
    }

}
