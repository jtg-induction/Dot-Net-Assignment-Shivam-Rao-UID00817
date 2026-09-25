using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto model, CancellationToken cancellationToken = default);

        Task<TokenResultDto> LoginAsync(LoginRequestDto model, CancellationToken cancellationToken = default);

        Task<TokenResultDto> RotateTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

        Task<bool> LogoutAsync(string refreshToken, CancellationToken cancellationToken = default);

        Task LogOutFromAllDevicesAsync(long userId, CancellationToken cancellationToken = default);

    }
}
