using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Utils;
using Microsoft.Owin;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Middlewares
{
    public class AuthenticationMiddleware : OwinMiddleware
    {
        public AuthenticationMiddleware(OwinMiddleware next) : base(next)
        {

        }

        public override async Task Invoke(IOwinContext context)
        {
            string path = context.Request.Path.Value;

            if (Constants.PublicPaths.IsPublicPath(path))
            {
                await Next.Invoke(context);
                return;
            }

            string authHeader = context.Request.Headers.Get("Authorization");

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer " , StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = (Int16)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Authorization token is required");
                return;
            }

            string token = authHeader.Substring("Bearer ".Length).Trim();

            TokenPayloadDto payload = await TokenValidatorAndVerifier.ValidateToken(token);

            if (payload == null)
            {
                context.Response.StatusCode = (Int16)HttpStatusCode.Unauthorized;
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
