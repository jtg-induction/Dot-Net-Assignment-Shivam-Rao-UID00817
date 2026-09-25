using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs
{
    public class OwnerOnboardRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public List<string> Emails { get; set; } = new List<string>();

        public OwnerOnboardRequestDto(string name, List<string> emails)
        {
            Name = name;
            Emails.AddRange(emails);
        }

        public OwnerOnboardRequestDto()
        {

        }
    }
}
