using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ValidationException = Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions.ValidationException;


namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Restaurant_ManagementContext _db;

        public UserRepository(Restaurant_ManagementContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Checks whether a user with the specified email exists.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>True if the email exists; otherwise, false.</returns>
        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _db.Users.AnyAsync(u => u.Email == email);
        }

        /// <summary>
        /// Checks whether a user with the specified phone number exists.
        /// </summary>
        /// <param name="phoneNumber">The phone number to check.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>True if the phone number exists; otherwise, false.</returns>
        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            return await _db.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
        }

        /// <summary>
        /// Adds a user to the database context.
        /// </summary>
        /// <param name="user">The user to add.</param>
        public void Add(Users user)
        {
            _db.Users.Add(user);
        }

        /// <summary>
        /// Retrieves active users matching the specified email addresses.
        /// </summary>
        /// <param name="emails">The email addresses to search for.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>A list of matching active users.</returns>
        public async Task<List<Users>> GetUsersByEmails(List<string> emails, CancellationToken cancellationToken = default)
        {
            return await _db.Users.Where(x => emails.Contains(x.Email) && x.IsActive).ToListAsync();
        }

        /// <summary>
        /// Retrieves a user by email, with optional change tracking.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="enableTracking">Whether to enable entity tracking.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        public async Task<Users> GetUserByEmailAsync(string email, bool enableTracking = false, CancellationToken cancellationToken = default)
        {
            if (enableTracking)
                return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            else
                return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Retrieves a user by ID, with optional change tracking.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="enableTracking">Whether to enable entity tracking.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        public async Task<Users> GetUserByUserIdAsync(long userId, bool enableTracking = false, CancellationToken cancellationToken = default)
        {
            if (enableTracking)
                return await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            else
                return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
        }


        /// <summary>
        /// Deactivates a user by setting their active status to false.
        /// </summary>
        /// <param name="userId">The ID of the user to deactivate.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        public async Task DeactivateUserAsync(long userId, CancellationToken cancellationToken = default)
        {
            (await _db.Users.FindAsync(userId)).IsActive = false;
        }

        /// <summary>
        /// Retrieves a user by ID while applying an update and row-level lock.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve and lock.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The locked user.</returns>
        /// <exception cref="ValidationException">
        /// Thrown when the specified user does not exist.
        /// </exception>
        public async Task<Users> GetUserWithUpdateLockAsync(long userId, CancellationToken cancellationToken = default)
        {
            var UserId = new SqlParameter("@p0", userId);
            return await _db.Users.SqlQuery("SELECT user_id AS UserId," +
                                                    "email AS Email," +
                                                    "phone_number AS PhoneNumber," +
                                                    "password AS Password," +
                                                    "name AS Name," +
                                                    "wallet_balance AS WalletBalance," +
                                                    "is_active AS IsActive," +
                                                    "created_at AS CreatedAt," +
                                                    "updated_at AS UpdatedAt," +
                                                    "role AS Role" +
                                                    " FROM Users WITH(UPDLOCK, ROWLOCK) WHERE user_id = @p0;", UserId).FirstOrDefaultAsync() ?? throw new ValidationException(ErrorMessages.USER_DOES_NOT_EXIST);
        }
    }
}
