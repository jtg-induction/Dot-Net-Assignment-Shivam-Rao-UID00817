using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto model);

        Task<TokenResultDto> LoginAsync(LoginRequestDto model);

        Task<TokenResultDto> RotateTokenAsync(string refreshToken);

        Task<bool> LogoutAsync(string refreshToken);

        Task LogOutFromAllDevicesAsync(long userId);

    }
}
