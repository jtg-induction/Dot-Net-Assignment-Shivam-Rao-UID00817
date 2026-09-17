using Dot_Net_Assignment_Shivam_Rao_UID00817.Middlewares;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Text;

[assembly: OwinStartup(typeof(Dot_Net_Assignment_Shivam_Rao_UID00817.Startup))]
namespace Dot_Net_Assignment_Shivam_Rao_UID00817
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var secret = Environment.GetEnvironmentVariable("JWT_SECRET");
            var key = Encoding.UTF8.GetBytes(secret);

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active ,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true ,
                    ValidateAudience = true ,
                    ValidateIssuerSigningKey = true ,
                    ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ,
                    ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }
            });

            app.Use(typeof(AuthenticationMiddleware));

        }
    }
}
