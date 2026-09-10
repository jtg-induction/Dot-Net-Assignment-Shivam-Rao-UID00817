using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    public class Menus
    {
        [Key]
        public long menu_id { get; set; }
        public long restaurant_id { get; set; }
        [ForeignKey("restaurant_id")]
        public virtual Restaurants Restaurants { get; set; }
        [Required]
        public string name { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
