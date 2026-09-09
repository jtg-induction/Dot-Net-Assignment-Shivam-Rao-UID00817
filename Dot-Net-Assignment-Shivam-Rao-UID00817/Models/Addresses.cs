using System;
using System.ComponentModel.DataAnnotations;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    public class Addresses
    {
        [Key]
        public long address_id { get; set; }
        [Required]
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string state { get; set; }
        [Required]
        public string pincode { get; set; }
        [Required]
        public string country { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
