using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
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
        public async Task AddTokenAsync(Refresh_Tokens RefreshToken)
        {
            _db.Refresh_Tokens.Add(RefreshToken);
        }

        public async Task<Refresh_Tokens> CheckIfRefreshTokenExistsAsync(string token)
        {
            return await _db.Refresh_Tokens.FirstOrDefaultAsync(u => u.RefreshToken == token);
        }

        public async Task DeleteRefreshTokenAsync(Refresh_Tokens TokenRecord)
        {
            _db.Refresh_Tokens.Remove(TokenRecord);
        }
    }
}
