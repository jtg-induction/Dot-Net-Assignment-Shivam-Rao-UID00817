using System;
using System.ComponentModel.DataAnnotations;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    public class Items
    {
        [Key]
        public long item_id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public float price { get; set; }
        [Required]
        public int available_quantity { get; set; }

        public DateTime created_at { get; set; }
    }
}
