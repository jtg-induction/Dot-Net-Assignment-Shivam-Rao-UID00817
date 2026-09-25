using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using System.ComponentModel.DataAnnotations;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class RegisterDto
    {
        [Required]
        [RegularExpression(Regex.EMAIL_REGEX, ErrorMessage = ErrorMessages.INVALID_EMAIL_FORMAT)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(Regex.PASSWORD_REGEX, ErrorMessage = ErrorMessages.INVALID_PASSWORD_FORMAT)]
        public string Password { get; set; }

        [StringLength(15, MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
