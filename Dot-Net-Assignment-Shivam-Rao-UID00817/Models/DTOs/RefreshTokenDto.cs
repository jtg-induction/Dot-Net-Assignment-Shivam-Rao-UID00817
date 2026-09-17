using System.ComponentModel.DataAnnotations;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class RefreshTokenDto
    {
        [Required]
        public string Token { get; set; }
    }
}
