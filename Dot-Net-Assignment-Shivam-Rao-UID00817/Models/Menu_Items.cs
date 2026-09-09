using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    public class Menu_Items
    {
        public long menu_id { get; set; }
        [ForeignKey("menu_id")]
        public virtual Menus Menus { get; set; }
        public long item_id { get; set; }
        [ForeignKey("item_id")]
        public virtual Items Items { get; set; }

    }
}
