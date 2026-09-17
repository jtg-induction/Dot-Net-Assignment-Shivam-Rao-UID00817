using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Web;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Newtonsoft.Json.Serialization;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class RegisterDto
    {
        [Required]
        [RegularExpression(Regex.EMAIL_REGEX , ErrorMessage = ExceptionMessages.INVALID_EMAIL_FORMAT)]
        public string Email { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 8)]
        [RegularExpression(Regex.PASSWORD_REGEX, ErrorMessage = ExceptionMessages.INVALID_PASSWORD_FORMAT)]
        public string Password { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
