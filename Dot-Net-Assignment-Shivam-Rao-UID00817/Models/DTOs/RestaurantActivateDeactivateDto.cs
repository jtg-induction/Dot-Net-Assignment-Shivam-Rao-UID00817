using System.ComponentModel.DataAnnotations;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class RestaurantActivateDeactivateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
