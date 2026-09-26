using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces
{
    public interface IAdminRestaurantService
    {
        Task<List<string>> GetValidEmailsAsync(List<string> Emails , List<EmailAndStatus> status);
        Task<OwnerOnboardResponseDto> OnboardRestaurantAsync(RestaurantOnboardDto restaurant);
        Task AssignOwnerToRestaurantAsync(string Name , List<Users> ValidUsers , List<EmailAndStatus> Status);
        Task<OwnerOnboardResponseDto> AssignOwnerToRestaurantAsync(OwnerOnboardRequestDto model);
        Task DeactivateRestaurant(string name);
        Task ActivateRestaurant(string name);

    }
}
