using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models
{
    public class User_Address_Type
    {
        public long user_id { get; set; }
        [ForeignKey("user_id")]
        public virtual Users Users { get; set; }

        public long address_id { get; set; }
        [ForeignKey("address_id")]
        public virtual Addresses Addresses { get; set; }

        public string address_type { get; set; } = "Home";
    }
}
