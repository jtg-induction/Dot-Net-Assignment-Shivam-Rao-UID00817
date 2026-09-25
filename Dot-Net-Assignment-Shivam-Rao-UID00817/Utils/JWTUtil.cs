using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Utils
{
    public static class JWTUtil
    {
        private readonly static string secret = Environment.GetEnvironmentVariable("JWT_SECRET");
        private readonly static string issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        private readonly static string audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        private readonly static SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        /// <summary>
        /// Utility to generate the access token
        /// </summary>
        /// <param name="email">The email for which the token is to be generated.</param>
        /// <param name="userId">The user id for which the token is to be generated.</param>
        /// <param name="role">The role to be set within the token.</param>
        /// <returns>The generated access token string.</returns>
        public static string GenerateAccessToken(string email, long userId, Constants.Enums.Roles role)
        {
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, email) ,
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", userId.ToString()),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            var token = new JwtSecurityToken(issuer, audience, claims,
                expires: DateTime.UtcNow.AddSeconds(NumberConstants.JWT_EXPIRES_IN_SECONDS), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generate a new refresh token
        /// </summary>
        /// <returns>A refresh token.</returns>
        public static string GenerateRefreshToken()
        {
            var randomNumber = new Byte[32];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomNumber);
                string refreshToken = Convert.ToBase64String(randomNumber);
                return refreshToken;
            }
        }

        /// <summary>
        /// Utility to Validate the received access token.
        /// </summary>
        /// <param name="token">The received access token.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>An object that contains the payload of the access token.</returns>
        public static async Task<TokenPayloadDto> ValidateTokenAndGetPayloadAsync(string token, CancellationToken cancellationToken = default)
        {
            var tokenHandler = new JsonWebTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = true
            };

            var result = await tokenHandler.ValidateTokenAsync(token, validationParameters);

            if (result.IsValid)
            {
                string role = result.ClaimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
                string email = result.ClaimsIdentity.FindFirst(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub)?.Value;
                string userIdStr = result.ClaimsIdentity.FindFirst("userId")?.Value;
                long userId = !string.IsNullOrEmpty(userIdStr) ? Convert.ToInt64(userIdStr) : 0;

                TokenPayloadDto payload = new TokenPayloadDto
                {
                    Role = role,
                    Email = email,
                    UserId = userId
                };

                return payload;
            }
            else
            {
                Debug.WriteLine("returning null payload");
                return null;
            }
        }
    }
}
