using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Helpers;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Utils;
using System;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services
{
    public class AuthService : IAuthService
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IUserRepository _userRepository;

        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(IUserRepository userRepository , IRefreshTokenRepository refreshTokenRepository , IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task RegisterAsync(RegisterDto model)
        {
            string email = model.Email.Trim().ToLower();
            string phoneNumber = model.PhoneNumber.Trim();

            bool phoneNumberExists = await _userRepository.PhoneNumberExistsAsync(phoneNumber);
            bool emailExists = await _userRepository.EmailExistsAsync(email);


            if (emailExists) throw new ConflictException(ErrorMessages.USER_ALREADY_EXISTS);
            else if (phoneNumberExists) throw new ConflictException(ErrorMessages.DUPLICATE_PHONE_NUMBER);

            var newUser = new Users(email , phoneNumber , model.Password , model.Name);

            _userRepository.Add(newUser);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<TokenResultDto> LoginAsync(LoginRequestDto model)
        {
            string email = model.Email.Trim().ToLower();
            var user = (await _userRepository.GetUserByEmailAsync(email , true)) ?? throw new ValidationException
                (
                    ErrorMessages.INVALID_CREDENTIALS
                );

            bool passwordValid = HashingHelper.VerifyPassword(model.Password , user.Password);

            if (!passwordValid)
            {
                throw new ValidationException(
                    ErrorMessages.INVALID_CREDENTIALS
                );
            }

            if (user.IsActive == false)
            {
                user.IsActive = true;
            }

            string accessToken = JWTUtil.GenerateAccessToken(email , user.UserId , user.Role);
            string refreshToken = JWTUtil.GenerateRefreshToken();

            _refreshTokenRepository.Add(new Refresh_Tokens(user.UserId , refreshToken));

            await _unitOfWork.SaveChangesAsync();

            return new TokenResultDto(accessToken , refreshToken);
        }

        public async Task<TokenResultDto> RotateTokenAsync(string refreshToken)
        {
            var existingToken = await _refreshTokenRepository.GetRefreshTokenExistsAsync(refreshToken, false);

            if (existingToken == null) throw new Exceptions.ValidationException(ErrorMessages.INVALID_REFRESH_TOKEN);

            _refreshTokenRepository.DeleteRefreshToken(existingToken);

            if (existingToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exceptions.ValidationException(ErrorMessages.INVALID_REFRESH_TOKEN);
            }

            var user = await _userRepository.GetUserByUserIdAsync(existingToken.UserId , true);

            var accessToken = JWTUtil.GenerateAccessToken(
                user.Email ,
                user.UserId ,
                user.Role
            );

            var newRefreshToken = JWTUtil.GenerateRefreshToken();

            _refreshTokenRepository.Add(new Refresh_Tokens(user.UserId , newRefreshToken));

            await _unitOfWork.SaveChangesAsync();

            return new TokenResultDto(accessToken , refreshToken);
        }

        public async Task<bool> LogoutAsync(string refreshToken)
        {
            bool res = await _refreshTokenRepository.RemoveIfTokenExistsAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return res;
        }

        public async Task LogOutFromAllDevicesAsync(long userId)
        {
            await _refreshTokenRepository.RemoveAllTokensForUserIdAsync(userId);
        }
    }
}
