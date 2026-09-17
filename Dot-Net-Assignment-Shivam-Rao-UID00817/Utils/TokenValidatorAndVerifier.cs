using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Utils
{
    public class TokenValidatorAndVerifier
    {
        private static string secret = Environment.GetEnvironmentVariable("JWT_SECRET");
        private static string issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        private static string audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

        public static async Task<TokenPayloadDto> ValidateToken(string token)
        {
            var tokenHandler = new JsonWebTokenHandler();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true ,
                ValidIssuer = issuer ,
                ValidateAudience = true ,
                ValidAudience = audience ,
                ValidateIssuerSigningKey = true ,
                IssuerSigningKey = securityKey ,
                ValidateLifetime = true
            };

            var result = await tokenHandler.ValidateTokenAsync(token , validationParameters);

            if (result.IsValid)
            {
                string role = result.ClaimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
                string email = result.ClaimsIdentity.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                string userIdStr = result.ClaimsIdentity.FindFirst("userId")?.Value;
                long userId = !string.IsNullOrEmpty(userIdStr) ? Convert.ToInt64(userIdStr) : 0;

                TokenPayloadDto payload = new TokenPayloadDto
                {
                    Role = role ,
                    Email = email ,
                    UserId = userId
                };

                return payload;
            }
            else
            {
                return null;
            }
        }
    }
}
