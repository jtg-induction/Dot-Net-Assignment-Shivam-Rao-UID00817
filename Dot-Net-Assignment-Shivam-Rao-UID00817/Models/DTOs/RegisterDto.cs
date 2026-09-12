using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^\s*[^@\s]+@[^@\s]+\.[^@\s]+\s*$" , ErrorMessage = "Email must be a valid format (e.g., user@example.com).")]
        public string Email { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be between 8 - 15 characters in length and must contain atleast a digit, an uppercase letter, a lowercase letter and a special character")]
        public string Password { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
