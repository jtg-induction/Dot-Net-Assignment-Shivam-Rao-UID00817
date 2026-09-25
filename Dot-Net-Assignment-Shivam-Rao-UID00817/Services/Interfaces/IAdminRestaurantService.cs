using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces
{
    public interface IAdminRestaurantService
    {
        Task<List<string>> GetValidEmailsAsync(List<string> Emails, List<EmailAndStatus> status, CancellationToken cancellationToken = default);
        Task<OwnerOnboardResponseDto> OnboardRestaurantAsync(RestaurantOnboardDto restaurant, CancellationToken cancellationToken = default);
        Task AssignOwnerToRestaurantAsync(string Name, List<Users> ValidUsers, List<EmailAndStatus> Status, CancellationToken cancellationToken = default);
        Task<OwnerOnboardResponseDto> AssignOwnerToRestaurantAsync(OwnerOnboardRequestDto model, CancellationToken cancellationToken = default);
        Task DeactivateRestaurant(string name, CancellationToken cancellationToken = default);
        Task ActivateRestaurant(string name, CancellationToken cancellationToken = default);

    }
}
