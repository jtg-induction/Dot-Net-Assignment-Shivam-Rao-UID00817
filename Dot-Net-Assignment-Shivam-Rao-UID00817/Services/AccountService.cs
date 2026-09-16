using Dot_Net_Assignment_Shivam_Rao_UID00817.Helpers;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services
{
    public class AccountService: IAccountService
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IUserRepository _userRepository;

        private readonly IRefreshTokenRepository _refreshTokenRepository;

        private readonly IAuthService _authService;

        public AccountService(IUserRepository userRepository , IRefreshTokenRepository refreshTokenRepository , IUnitOfWork unitOfWork, IAuthService authService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public async Task DeactivateAccountAsync(long userId)
        {
            await _userRepository.SwitchUserIsActiveAsync(userId);
            await _authService.LogOutFromAllDevicesAsync(userId);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(long userId, UpdateAccountDto model)
        {
            if(model is null)
            {
                return;
            }
            Users user = await _userRepository.GetUserByUserIdAsync(userId);
            bool updated = false;
            if(model.Name != null)
            {
                updated = true;
                user.Name = model.Name;
            }
            if(model.Password != null)
            {
                updated = true;
                user.Password = HashingHelper.HashPassword(model.Password);
            }
            if(model.PhoneNumber != null)
            {
                updated = true;
                user.PhoneNumber = model.PhoneNumber;
            }

            if(updated) user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
