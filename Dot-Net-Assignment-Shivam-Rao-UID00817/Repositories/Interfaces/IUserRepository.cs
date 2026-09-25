using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> PhoneNumberExistsAsync(string phoneNumber, CancellationToken cancellationToken = default);
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
        void Add(Users user);
        Task<Users> GetUserByEmailAsync(string email, bool enableTracking, CancellationToken cancellationToken = default);
        Task<Users> GetUserByUserIdAsync(long userId, bool enableTracking, CancellationToken cancellationToken = default);
        Task<List<Users>> GetUsersByEmails(List<string> emails, CancellationToken cancellationToken = default);
        Task DeactivateUserAsync(long userId, CancellationToken cancellationToken = default);
        Task<Users> GetUserWithUpdateLockAsync(long userId, CancellationToken cancellationToken = default);
    }
}
