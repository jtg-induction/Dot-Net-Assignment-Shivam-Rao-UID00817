using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    public enum order_status
    {
        Placed = 0,
        Accepted = 1,
        Rejected = 2,
        Dispatched = 3,
        Delivered = 4,
        Cancelled = 5
    }
    public class Orders
    {
        [Key]
        public long order_id { get; set; }
        public string instructions { get; set; }

        public order_status status { get; set; } = order_status.Placed;

        [Required]
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string state { get; set; }
        [Required]
        public long pincode { get; set; }
        [Required]
        public string country { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public long user_id { get; set; }
        [ForeignKey("user_id")]
        public virtual Users Users { get; set; }

    }
}
