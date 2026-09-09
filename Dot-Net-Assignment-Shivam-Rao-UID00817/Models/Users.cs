using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Index("IX _User_Email", IsUnique = true)]
        [StringLength(255)]
        public string email { get; set; }
        [Index("IX _User_PhoneNumber", IsUnique = true)]
        [StringLength(50)]
        public string phone_number { get; set; }
        public string password { get; set; }
        public string name { get; set; }

        public User_role role { get; set; } = User_role.Customer;
        public decimal wallet_balance { get; set; } = 1000.0m;
        public bool is_active { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

    }
}
