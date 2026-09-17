using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System.Data.Entity;
using System.Linq;
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
        public void Add(Refresh_Tokens RefreshToken)
        {
            _db.Refresh_Tokens.Add(RefreshToken);
        }

        public async Task<Refresh_Tokens> CheckIfRefreshTokenExistsAsync(string token)
        {
            return await _db.Refresh_Tokens.FirstOrDefaultAsync(u => u.RefreshToken == token);
        }

        public void DeleteRefreshToken(Refresh_Tokens TokenRecord)
        {
            _db.Refresh_Tokens.Remove(TokenRecord);
        }

        public async Task<bool> RemoveIfTokenExistsAsync(string token)
        {
            var TokenRecord = await _db.Refresh_Tokens.FirstOrDefaultAsync(u => u.RefreshToken == token);

            if (TokenRecord == null) return false;

            _db.Refresh_Tokens.Remove(TokenRecord);

            return true;
        }

        public async Task RemoveAllTokensForUserIdAsync(long userId)
        {
            var TokenRecords = await _db.Refresh_Tokens.Where(u => u.UserId == userId).ToListAsync();

            _db.Refresh_Tokens.RemoveRange(TokenRecords);
        }
    }
}
