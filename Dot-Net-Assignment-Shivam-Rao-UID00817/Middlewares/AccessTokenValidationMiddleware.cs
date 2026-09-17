using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Utils;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Middlewares
{
    public class AccessTokenValidationMiddleware: OwinMiddleware
    {
        public AccessTokenValidationMiddleware(OwinMiddleware next): base(next)
        {

        }

        public override async Task Invoke(IOwinContext context)
        {
            string path = context.Request.Path.Value;

            if (path.Equals("/", StringComparison.OrdinalIgnoreCase) ||
                path.Equals("/api/auth/login" , StringComparison.OrdinalIgnoreCase) ||
                path.Equals("/api/auth/refresh" , StringComparison.OrdinalIgnoreCase) ||
                path.Equals("/api/auth/register" , StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
            {
                await Next.Invoke(context);
                return;
            }

            string authHeader = context.Request.Headers.Get("Authorization");

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer " , StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Authorization token is required");
                return;
            }

            string token = authHeader.Substring("Bearer ".Length).Trim();

            TokenPayloadDto payload = await TokenValidatorAndVerifier.ValidateToken(token);

            if(payload == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Authorization token is required");
                return;
            }

            var identity = new ClaimsIdentity("Bearer");

            identity.AddClaim(
                new Claim("userId" , payload.UserId.ToString())
            );

            identity.AddClaim(
                new Claim(ClaimTypes.Email , payload.Email)
            );

            identity.AddClaim(
                new Claim(ClaimTypes.Role , payload.Role)
            );

            context.Request.User = new ClaimsPrincipal(identity);

            await Next.Invoke(context);
        }
    }
}
