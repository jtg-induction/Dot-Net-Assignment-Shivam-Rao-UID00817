using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    public enum User_role
    {
        Customer = 0,
        Owner = 1,
        Admin = 2
    }
    public class Users
    {
        [Key]
        public long user_id { get; set; }
        [Required]
        [Index(nameof(email), IsUnique=true)]
        public string email { get; set; }
        [Required]
        [Index(nameof(phone_number), IsUnique=true)]
        public string phone_number { get; set; }
        public string name { get; set; }

        public User_role role { get; set; } = User_role.Customer;
        public float wallet_balance { get; set; } = 1000.0f;
        public bool is_active { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

    }
}
