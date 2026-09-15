using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Utils;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
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
            string authHeader = context.Request.Headers.Get("Authorization");

            if(!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                string token = authHeader.Substring("Bearer ".Length).Trim();

                var payload = TokenValidatorAndVerifier.ValidateToken(token);

                if(payload == null)
                {
                    return;
                }

            }
            await Next.Invoke(context);
        }
    }
}
