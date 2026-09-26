using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> PhoneNumberExistsAsync(string phoneNumber);
        Task<bool> EmailExistsAsync(string email);
        void Add(Users user);
        Task<Users> GetUserByEmailAsync(string email , bool enableTracking);
        Task<Users> GetUserByUserIdAsync(long userId , bool enableTracking);
        Task<List<Users>> GetUsersByEmails(List<string> emails);
        Task DeactivateUserAsync(long userId);
        Task<Users> GetUserWithUpdateLockAsync(long userId);
    }
}
