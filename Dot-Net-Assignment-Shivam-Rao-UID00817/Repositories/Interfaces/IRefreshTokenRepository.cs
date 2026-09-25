using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        void Add(Refresh_Tokens RefreshToken);

        Task<Refresh_Tokens> GetRefreshTokenExistsAsync(string token, bool AsNoTracking, CancellationToken cancellationToken = default);

        void DeleteRefreshToken(Refresh_Tokens TokenRecord);

        Task<bool> RemoveIfTokenExistsAsync(string token, CancellationToken cancellationToken = default);

        Task RemoveAllTokensForUserIdAsync(long userId, CancellationToken cancellationToken = default);
    }
}
