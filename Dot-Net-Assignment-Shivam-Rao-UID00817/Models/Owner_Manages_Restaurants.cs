using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    public class Owner_Manages_Restaurants
    {
        public long restaurant_id { get; set; }
        [ForeignKey("restaurant_id")]
        public virtual Restaurants Restaurants { get; set; }
        public long user_id { get; set; }
        [ForeignKey("user_id")]
        public virtual Users Users { get; set; }

        public DateTime created_at { get; set; }
    }
}
