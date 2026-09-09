using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    public class Order_Items
    {
        [Key]
        [Column(Order = 1)]
        public long order_id { get; set; }
        [ForeignKey("order_id")]
        public virtual Orders Orders { get; set; }

        [Key]
        [Column(Order = 2)]
        public long item_id { get; set; }
        [ForeignKey("item_id")]
        public virtual Items Items { get; set; }

        public float item_price { get; set; }
        public int quantity { get; set; }
    }
}
