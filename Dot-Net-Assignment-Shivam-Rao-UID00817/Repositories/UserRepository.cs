using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System.Data.Entity;
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

        public async Task<bool> UserExistsAsync(string email , string phoneNumber)
        {
            return await _db.Users.AnyAsync(u => u.Email == email || u.PhoneNumber == phoneNumber);
        }

        public void Add(Users user)
        {
            _db.Users.Add(user);
        }

        public async Task<Users> GetUserByEmailAsync(string email , bool AsNoTracking)
        {
            if (AsNoTracking)
                return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            else
                return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Users> GetUserByUserIdAsync(long userId , bool AsNoTracking)
        {
            if (AsNoTracking)
                return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
            else
                return await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }
}
