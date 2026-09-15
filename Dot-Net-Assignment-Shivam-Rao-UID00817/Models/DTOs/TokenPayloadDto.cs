using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class TokenPayloadDto
    {
        public string Email { get; set; }
        public long UserId { get; set; }
        public string Role { get; set; }
    }
}
