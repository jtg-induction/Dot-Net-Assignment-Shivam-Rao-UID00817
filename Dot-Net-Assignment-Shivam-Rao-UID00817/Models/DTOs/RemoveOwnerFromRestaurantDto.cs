using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class RemoveOwnerFromRestaurantDto
    {
        [Required]
        public string Name { get; set; }
        [Required, RegularExpression(Constants.Regex.EMAIL_REGEX, ErrorMessage = ErrorMessages.INVALID_EMAIL_FORMAT)]
        public string Email { get; set; }
    }
}
