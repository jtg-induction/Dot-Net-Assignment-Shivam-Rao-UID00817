using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class LoginRequestDto
    {
        [Required]
        [RegularExpression(REGEX.EMAIL_REGEX , ErrorMessage = ERROR_MESSAGES.INVALID_EMAIL_FORMAT)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
