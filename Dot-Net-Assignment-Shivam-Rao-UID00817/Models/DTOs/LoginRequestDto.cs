using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using System.ComponentModel.DataAnnotations;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class LoginRequestDto
    {
        [Required]
        [RegularExpression(Regex.EMAIL_REGEX , ErrorMessage = ErrorMessages.INVALID_EMAIL_FORMAT)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
