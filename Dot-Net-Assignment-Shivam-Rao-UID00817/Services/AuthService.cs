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
using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using System.Net;
using Microsoft.Owin.Security;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Helpers;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services
{
    public class AuthService: IAuthService
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IUserRepository _userRepository;

        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task RegisterAsync(RegisterDto model)
        {
            string email = model.Email.Trim().ToLower();
            string phoneNumber = model.PhoneNumber.Trim();

            bool userExists = await _userRepository.UserExistsAsync(email, phoneNumber);

            if (userExists)
            {
                throw new ConflictException(
                    EXCEPTION_MESSAGES.USER_ALREADY_EXISTS
                );
            }

            var newUser = new Users(email , phoneNumber , model.Password , model.Name);

            _userRepository.Add(newUser);

            await _unitOfWork.SaveChangesAsync();
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
                    EXCEPTION_MESSAGES.INVALID_CREDENTIALS
                );

            bool passwordValid = HashingHelper.VerifyPassword(model.Password , user.Password);

            if (!passwordValid)
            {
                throw new ValidationException(
                    EXCEPTION_MESSAGES.INVALID_CREDENTIALS
                );
            }

            string accessToken = TokenGenerator.GenerateAccessToken(email , user.UserId , user.Role);
            string refreshToken = TokenGenerator.GenerateRefreshToken();

            _refreshTokenRepository.Add(new Refresh_Tokens(user.UserId , refreshToken));

            await _unitOfWork.SaveChangesAsync();

            return new TokenResult
            {
                AccessToken =  accessToken ,
                RefreshToken = refreshToken
            };
        }

        public async Task<ITokenResult> RotateTokenAsync(string refreshToken)
        {
            var existingToken = await _refreshTokenRepository.CheckIfRefreshTokenExistsAsync(refreshToken);

            if (existingToken == null) throw new Exceptions.ValidationException(EXCEPTION_MESSAGES.INVALID_REFRESH_TOKEN);

            _refreshTokenRepository.DeleteRefreshToken(existingToken);

            if(existingToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exceptions.ValidationException(EXCEPTION_MESSAGES.INVALID_REFRESH_TOKEN);
            }

            var user = await _userRepository.GetUserByUserIdAsync(existingToken.UserId);

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
            bool res = await _refreshTokenRepository.RemoveTokenAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return res;
        }
    }
}
