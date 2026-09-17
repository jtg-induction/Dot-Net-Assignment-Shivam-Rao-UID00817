using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> DuplicatePhoneNumberExistsAsync(string phoneNumber);
        Task<bool> DuplicateEmailExistsAsync(string email);
        void Add(Users user);
        Task<Users> GetUserByEmailAsync(string email , bool AsNoTracking);
        Task<Users> GetUserByUserIdAsync(long userId , bool AsNoTracking);
    }
}
