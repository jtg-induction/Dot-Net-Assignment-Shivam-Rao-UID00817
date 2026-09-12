using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly Restaurant_ManagementContext _db;

        public UserRepository(Restaurant_ManagementContext db)
        {
            _db = db;
        }

        public async Task<bool> UserExistsAsync(string email, string phoneNumber)
        {
            return await _db.Users.AnyAsync(u => u.Email == email && u.PhoneNumber == phoneNumber);
        }

        public void AddUser(Users user)
        {
            _db.Users.Add(user);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

    }
}
