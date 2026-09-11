using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    [Table("Menu_Items")]
    public class Menu_Items
    {
        [Key]
        [Column("menu_id", Order = 1)]
        public long MenuId { get; set; }

        [ForeignKey(nameof(MenuId))]
        public virtual Menus Menus { get; set; }

        [Key]
        [Column("item_id", Order = 2)]
        public long ItemId { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual Items Items { get; set; }
    }
}
