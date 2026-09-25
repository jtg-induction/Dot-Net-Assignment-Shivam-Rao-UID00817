using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Helpers;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Utils;
using System;
using System.Threading;
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

        /// <summary>
        /// Registers the user with given details.
        /// </summary>
        /// <param name="model">The information of the user to be registered.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        public async Task RegisterAsync(RegisterDto model, CancellationToken cancellationToken = default)
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
        
        /// <summary>
        /// Logs in the user with the credentials.
        /// </summary>
        /// <param name="model">The credentials for logging in.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>An object with the access token and the Expiry time.</returns>
        public async Task<TokenResultDto> LoginAsync(LoginRequestDto model, CancellationToken cancellationToken = default)
        {
            string email = model.Email.Trim().ToLower();
            var user = (await _userRepository.GetUserByEmailAsync(email , false)) ?? throw new ValidationException
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

        /// <summary>
        /// Uses the refresh token to generate a new access token, while rotating the refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token stored in HttpOnly Cookie.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>An object with the access token and the Expiry time.</returns>
        public async Task<TokenResultDto> RotateTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var existingToken = await _refreshTokenRepository.GetRefreshTokenExistsAsync(refreshToken , true);

            if (existingToken == null) throw new Exceptions.ValidationException(ErrorMessages.INVALID_REFRESH_TOKEN);

            _refreshTokenRepository.DeleteRefreshToken(existingToken);

            if (existingToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exceptions.ValidationException(ErrorMessages.INVALID_REFRESH_TOKEN);
            }

            var user = await _userRepository.GetUserByUserIdAsync(existingToken.UserId , false);

            var accessToken = JWTUtil.GenerateAccessToken(
                user.Email ,
                user.UserId ,
                user.Role
            );

            var newRefreshToken = JWTUtil.GenerateRefreshToken();

            _refreshTokenRepository.Add(new Refresh_Tokens(user.UserId , newRefreshToken));

            await _unitOfWork.SaveChangesAsync();

            return new TokenResultDto(accessToken , newRefreshToken);
        }

        /// <summary>
        /// Logs the user out, deleting the refresh token from the db.
        /// </summary>
        /// <param name="refreshToken">The refresh token stored in the HttpOnly Cookie.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>True when the refresh token was valid and the user was logged out successfully; Otherwise, false.</returns>
        public async Task<bool> LogoutAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            bool res = await _refreshTokenRepository.RemoveIfTokenExistsAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return res;
        }

        /// <summary>
        /// Removes all the refresh tokens associated to the userId from the db.
        /// </summary>
        /// <param name="userId">The User Id of the use to be logged out of all devices.</param>
        /// <param name="cancellationToken">Token used to cancel the operation. </param>
        public async Task LogOutFromAllDevicesAsync(long userId, CancellationToken cancellationToken = default)
        {
            await _refreshTokenRepository.RemoveAllTokensForUserIdAsync(userId);
        }
    }
}
