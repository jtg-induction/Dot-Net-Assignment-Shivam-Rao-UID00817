using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    public class Order_Items
    {
        public long order_id {  get; set; }
        [ForeignKey("order_id")]
        public virtual Orders Orders { get; set; }

        public long item_id { get; set; }
        [ForeignKey("item_id")]
        public virtual Items Items { get; set; }

        public float item_price { get; set; }
        public int quantity { get; set; }
    }
}
