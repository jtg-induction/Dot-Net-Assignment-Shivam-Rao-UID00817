using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly Restaurant_ManagementContext _db;

        public RefreshTokenRepository(Restaurant_ManagementContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Adds a refresh token to the database context.
        /// </summary>
        /// <param name="RefreshToken">The refresh token to add.</param>
        public void Add(Refresh_Tokens RefreshToken)
        {
            _db.Refresh_Tokens.Add(RefreshToken);
        }

        /// <summary>
        /// Retrieves a refresh token by its value, with optional change tracking.
        /// </summary>
        /// <param name="token">The refresh token value.</param>
        /// <param name="enableTracking">Whether to enable entity tracking.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The refresh token record if found; otherwise, null.</returns>
        public async Task<Refresh_Tokens> GetRefreshTokenExistsAsync(string token, bool enableTracking = false, CancellationToken cancellationToken = default)
        {
            if (enableTracking)
                return await _db.Refresh_Tokens.FirstOrDefaultAsync(u => u.RefreshToken == token);
            else
                return await _db.Refresh_Tokens.AsNoTracking().FirstOrDefaultAsync(u => u.RefreshToken == token);
        }

        /// <summary>
        /// Deletes a refresh token from the database context.
        /// </summary>
        /// <param name="TokenRecord">The refresh token record to delete.</param>
        public void DeleteRefreshToken(Refresh_Tokens TokenRecord)
        {
            _db.Refresh_Tokens.Remove(TokenRecord);
        }

        /// <summary>
        /// Removes a refresh token if it exists.
        /// </summary>
        /// <param name="token">The refresh token value.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>True if the token was found and removed; otherwise, false.</returns>
        public async Task<bool> RemoveIfTokenExistsAsync(string token, CancellationToken cancellationToken = default)
        {
            var TokenRecord = await _db.Refresh_Tokens.FirstOrDefaultAsync(u => u.RefreshToken == token);

            if (TokenRecord == null) return false;

            _db.Refresh_Tokens.Remove(TokenRecord);

            return true;
        }

        /// <summary>
        /// Removes all refresh tokens associated with a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        public async Task RemoveAllTokensForUserIdAsync(long userId, CancellationToken cancellationToken = default)
        {
            var TokenRecords = await _db.Refresh_Tokens.Where(u => u.UserId == userId).ToListAsync();

            _db.Refresh_Tokens.RemoveRange(TokenRecords);
        }
    }
}
