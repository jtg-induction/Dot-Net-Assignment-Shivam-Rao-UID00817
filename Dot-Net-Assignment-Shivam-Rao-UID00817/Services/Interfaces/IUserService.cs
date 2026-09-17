using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces
{
    public interface IUserService
    {
        Task DeactivateAccountAsync(long userId);

        Task UpdateAccountAsync(long userId , UpdateAccountDto model);
    }
}
