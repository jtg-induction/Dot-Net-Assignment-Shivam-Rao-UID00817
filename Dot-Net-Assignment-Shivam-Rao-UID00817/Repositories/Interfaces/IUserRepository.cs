using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> PhoneNumberExists(string phoneNumber, CancellationToken cancellationToken = default);
        Task<bool> EmailExists(string email, CancellationToken cancellationToken = default);
        void Add(Users user);
        Task<Users> GetUserByEmail(string email, bool enableTracking, CancellationToken cancellationToken = default);
        Task<Users> GetUserByUserId(long userId, bool enableTracking, CancellationToken cancellationToken = default);
        Task<List<Users>> GetValidActiveUsersByEmails(List<string> emails, CancellationToken cancellationToken = default);
        Task DeactivateUser(long userId, CancellationToken cancellationToken = default);
        Task<Users> GetUserWithUpdateLock(long userId, CancellationToken cancellationToken = default);
    }
}
