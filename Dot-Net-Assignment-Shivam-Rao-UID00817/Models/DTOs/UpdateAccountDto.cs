using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class UpdateAccountDto
    {
        [StringLength(15 , MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        public string Name { get; set; }
    }
}
