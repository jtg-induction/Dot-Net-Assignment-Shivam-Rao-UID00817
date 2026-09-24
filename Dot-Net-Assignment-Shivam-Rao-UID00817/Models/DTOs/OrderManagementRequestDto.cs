using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class OrderManagementRequestDto
    {
        public Enums.OrderStatus ChangeStatusTo { get; set; }
    }
}
