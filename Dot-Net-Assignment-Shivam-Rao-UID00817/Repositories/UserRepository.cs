using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;


namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Restaurant_ManagementContext _db;

        public UserRepository(Restaurant_ManagementContext db)
        {
            _db = db;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _db.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            return await _db.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
        }

        public void Add(Users user)
        {
            _db.Users.Add(user);
        }

        public async Task<List<Users>> GetUsersByEmails(List<string> emails)
        {
            return await _db.Users.Where(x => emails.Contains(x.Email) && x.IsActive).ToListAsync();
        }

        public async Task<Users> GetUserByEmailAsync(string email , bool enableTracking = false)
        {
            if (enableTracking)
                return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            else
                return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Users> GetUserByUserIdAsync(long userId , bool enableTracking = false)
        {
            if (enableTracking)
                return await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            else
                return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task DeactivateUserAsync(long userId)
        {
            (await _db.Users.FindAsync(userId)).IsActive = false;
        }

        public async Task<int> DeductWalletBalanceIfSufficientAsync(long userId, decimal amount)
        {
            return await _db.Database.ExecuteSqlCommandAsync(
                @"
                    UPDATE Users
                    SET wallet_balance = wallet_balance - @p0
                    WHERE user_id = @p1
                        AND wallet_balance >= @p0" ,
                amount , userId);
        }
    }
}
