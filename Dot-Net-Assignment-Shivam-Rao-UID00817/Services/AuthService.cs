using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Helpers;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
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
using Dot_Net_Assignment_Shivam_Rao_UID00817.Utils;
using System;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services
{
    public class AuthService : IAuthService
    public class AuthService : IAuthService
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IUserRepository _userRepository;

        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(IUserRepository userRepository , IRefreshTokenRepository refreshTokenRepository , IUnitOfWork unitOfWork)
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

            bool phoneNumberExists = await _userRepository.DuplicatePhoneNumberExistsAsync(phoneNumber);
            bool emailExists = await _userRepository.DuplicateEmailExistsAsync(email);


            if (phoneNumberExists && emailExists) throw new ConflictException(ExceptionMessages.USER_ALREADY_EXISTS);
            else if (phoneNumberExists) throw new ConflictException(ExceptionMessages.DUPLICATE_PHONE_NUMBER);
            else if (emailExists) throw new ConflictException(ExceptionMessages.DUPLICATE_EMAIL);

            var newUser = new Users(email , phoneNumber , model.Password , model.Name);

            _userRepository.Add(newUser);

            await _unitOfWork.SaveChangesAsync();
        }

        public class TokenResult : ITokenResult
        public class TokenResult : ITokenResult
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
        }

        public async Task<ITokenResult> LoginAsync(LoginRequestDto model)
        {
            string email = model.Email.Trim().ToLower();
            var user = (await _userRepository.GetUserByEmailAsync(email , true)) ?? throw new ValidationException
                (
                    ExceptionMessages.INVALID_CREDENTIALS
                );

            bool passwordValid = HashingHelper.VerifyPassword(model.Password , user.Password);

            if (!passwordValid)
            {
                throw new ValidationException(
                    ExceptionMessages.INVALID_CREDENTIALS
                );
            }

            if (user.IsActive == false)
            {
                await _userRepository.ToggleUserIsActiveAsync(user.UserId);
            }

            string accessToken = TokenGenerator.GenerateAccessToken(email , user.UserId , user.Role);
            string refreshToken = TokenGenerator.GenerateRefreshToken();

            _refreshTokenRepository.Add(new Refresh_Tokens(user.UserId , refreshToken));

            await _unitOfWork.SaveChangesAsync();

            return new TokenResult
            {
                AccessToken = accessToken ,
                AccessToken = accessToken ,
                RefreshToken = refreshToken
            };
        }

        public async Task<ITokenResult> RotateTokenAsync(string refreshToken)
        {
            var existingToken = await _refreshTokenRepository.GetRefreshTokenExistsAsync(refreshToken , false);

            if (existingToken == null) throw new Exceptions.ValidationException(ExceptionMessages.INVALID_REFRESH_TOKEN);

            _refreshTokenRepository.DeleteRefreshToken(existingToken);

            if (existingToken.ExpiresAt < DateTime.UtcNow)
            if (existingToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exceptions.ValidationException(ExceptionMessages.INVALID_REFRESH_TOKEN);
            }

            var user = await _userRepository.GetUserByUserIdAsync(existingToken.UserId , true);

            var accessToken = TokenGenerator.GenerateAccessToken(
                user.Email ,
                user.UserId ,
                user.Role
            );

            var newRefreshToken = TokenGenerator.GenerateRefreshToken();

            _refreshTokenRepository.Add(new Refresh_Tokens(user.UserId , newRefreshToken));

            await _unitOfWork.SaveChangesAsync();

            return new TokenResult
            {
                AccessToken = accessToken ,
                RefreshToken = newRefreshToken
            };
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
