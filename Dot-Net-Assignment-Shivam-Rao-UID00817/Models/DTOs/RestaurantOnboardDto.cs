using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class RestaurantOnboardDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required, MaxLength(12), MinLength(3)]
        public string Pincode { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public List<string> Emails { get; set; } = new List<string>();
    }
}
