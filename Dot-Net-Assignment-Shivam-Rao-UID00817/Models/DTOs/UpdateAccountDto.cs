using System.ComponentModel.DataAnnotations;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class UpdateAccountDto
    {
        [StringLength(15 , MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        public string Name { get; set; }
    }
}
