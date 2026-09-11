using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    [Table("Order_Items")]
    public class Order_Items
    {
        [Key]
        [Column("order_id", Order = 1)]
        public long OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Orders Orders { get; set; }

        [Key]
        [Column("item_id", Order = 2)]
        public long ItemId { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual Items Items { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("item_price")]
        public decimal ItemPrice { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }
    }
}
