using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    public class Restaurants
    {
        [Key]
        public long restaurant_id { get; set; }
        [Required]
        public string name { get; set; }
        public DateTime created_at { get; set; }

        public long address_id { get; set; }
        [ForeignKey("address_id")]
        public virtual Addresses Addresses { get; set; }

    }
}
