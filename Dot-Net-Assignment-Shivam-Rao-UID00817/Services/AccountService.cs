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
    }
}
