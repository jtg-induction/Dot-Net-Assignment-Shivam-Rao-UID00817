using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories
{
    public class RefreshTokenRepository: IRefreshTokenRepository
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

        public async Task<bool> RemoveTokenAsync(string token)
        {
            var TokenRecord = await _db.Refresh_Tokens.FirstOrDefaultAsync(u => u.RefreshToken == token);

            if (token == null) return false;

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
