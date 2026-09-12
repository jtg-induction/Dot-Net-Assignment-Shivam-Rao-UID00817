using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Utils;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services
{
    public class AuthService: IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> RegisterAsync(RegisterDto model)
        {
            if (model == null)
            {
                return false;
            }
            string email = model.Email.Trim().ToLower();
            string phoneNumber = model.PhoneNumber.Trim();

            bool userExists = await _userRepository.UserExistsAsync(email, phoneNumber);

            if (userExists)
            {
                return false;
            }

            DateTime currTime = DateTime.UtcNow;

            var newUser = new Users
            {
                Email = email ,
                Password = _passwordHasher.HashPassword(model.Password.Trim()),
                PhoneNumber = phoneNumber ,
                Name = model.Name.Trim() ,
                Role = "Customer",
                CreatedAt = currTime,
                UpdatedAt = currTime,
                WalletBalance = 1000m,
                IsActive = true
            };

            _userRepository.AddUser(newUser);

            await _userRepository.SaveChangesAsync();

            return true;
        }
    }
}
