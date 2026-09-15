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
using Microsoft.Owin;
using System.Web.Http.Results;
using System.Net.Http;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services
{
    public class AuthService: IAuthService
    {
        private readonly IUserRepository _userRepository;

        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task RegisterAsync(RegisterDto model)
        {
            string email = model.Email.Trim().ToLower();
            string phoneNumber = model.PhoneNumber.Trim();

            bool userExists = await _userRepository.UserExistsAsync(email, phoneNumber);

            if (userExists)
            {
                throw new ValidationException(
                    Constants.USER_ALREADY_EXISTS
                );
            }

            DateTime currTime = DateTime.UtcNow;

            var newUser = new Users(email , phoneNumber , model.Password , model.Name);

            await _userRepository.AddUserAsync(newUser);
        }

        public class TokenResult: ITokenResult
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
        }

        public async Task<ITokenResult> LoginAsync(LoginRequestDto model)
        {
            string email = model.Email.Trim().ToLower();
            var user = (await _userRepository.GetUserByEmailAsync(email)) ?? throw new ValidationException
                (
                    Constants.INVALID_CREDENTIALS
                );

            bool passwordValid = PasswordVerifier.VerifyPassword(model.Password , user.Password);

            if (!passwordValid)
            {
                throw new ValidationException(
                    Constants.INVALID_CREDENTIALS
                );
            }

            string accessToken = TokenGenerator.GenerateAccessToken(email , user.UserId , user.Role);
            string refreshToken = TokenGenerator.GenerateRefreshToken();

            DateTime currtime = DateTime.UtcNow;

            await _refreshTokenRepository.AddTokenAsync(new Refresh_Tokens(user.UserId , refreshToken));

            return new TokenResult
            {
                AccessToken =  accessToken ,
                RefreshToken = refreshToken
            };
        }

        public async Task<ITokenResult> RotateTokenAsync(string refreshToken)
        {
            var existingToken = await _refreshTokenRepository.ValidateTokenAndGetDetails(refreshToken);

            if (existingToken == null) return null;

            var user = await _userRepository.GetUserByUserIdAsync(existingToken.UserId);

            if (user == null) return null;

            var accessToken = TokenGenerator.GenerateAccessToken(
                user.Email ,
                user.UserId ,
                user.Role
            );

            var newRefreshToken = TokenGenerator.GenerateRefreshToken();

            await _refreshTokenRepository.AddTokenAsync(new Refresh_Tokens(user.UserId , newRefreshToken));

            return new TokenResult
            {
                AccessToken = accessToken ,
                RefreshToken = newRefreshToken
            };
        }
    }
}
