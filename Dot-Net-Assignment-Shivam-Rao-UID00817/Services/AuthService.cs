using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Utils;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services
{
    public class AuthService: IAuthService
    {
        private readonly IUserRepository _userRepository;

        //private readonly IPasswordHasher _passwordHasher;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task RegisterAsync(RegisterDto model)
        {
            string email = model.Email.Trim().ToLower();
            string phoneNumber = model.PhoneNumber.Trim();

            bool userExists = await _userRepository.UserExistsAsync(email, phoneNumber);

            if (userExists)
            {
                throw new UserAlreadyExistsException("Email / Phone Number already exists.");
            }

            DateTime currTime = DateTime.UtcNow;

            var newUser = new Users
            {
                Email = email ,
                Password = PasswordHasher.HashPassword(model.Password.Trim()),
                PhoneNumber = phoneNumber ,
                Name = model.Name.Trim() ,
                Role = "Customer",
                CreatedAt = currTime,
                UpdatedAt = currTime,
                WalletBalance = 1000m,
                IsActive = true
            };

            await _userRepository.AddUserAsync(newUser);
        }
    }
}
