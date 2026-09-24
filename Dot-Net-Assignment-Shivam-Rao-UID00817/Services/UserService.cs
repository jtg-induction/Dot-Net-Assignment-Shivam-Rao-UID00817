using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IUserRepository _userRepository;

        private readonly IRefreshTokenRepository _refreshTokenRepository;

        private readonly IAuthService _authService;

        public UserService(IUserRepository userRepository , IRefreshTokenRepository refreshTokenRepository , IUnitOfWork unitOfWork , IAuthService authService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public async Task DeactivateAccountAsync(long userId, CancellationToken cancellationToken = default)
        {
            await _userRepository.DeactivateUserAsync(userId);
            await _authService.LogOutFromAllDevicesAsync(userId);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(long userId , UpdateAccountDto model, CancellationToken cancellationToken = default)
        {
            if (model is null)
            {
                return;
            }
            Users user = await _userRepository.GetUserByUserIdAsync(userId, true);
            if (!String.IsNullOrWhiteSpace(model.Name))
            {
                user.Name = model.Name.Trim();
            }
            if (!String.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                if (await _userRepository.PhoneNumberExistsAsync(model.PhoneNumber.Trim()))
                    throw new ConflictException(ErrorMessages.DUPLICATE_PHONE_NUMBER);
                user.PhoneNumber = model.PhoneNumber.Trim();
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
