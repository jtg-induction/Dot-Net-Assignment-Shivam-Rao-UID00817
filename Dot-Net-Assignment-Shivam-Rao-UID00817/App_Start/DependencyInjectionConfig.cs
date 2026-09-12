using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Controllers;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.App_Start
{
    public static class DependencyInjectionConfig
    {
        public static IServiceProvider RegisterServices()
        {
            var services = new ServiceCollection();

            services.AddScoped<Restaurant_ManagementContext>();

            services.AddScoped<IUserRepository , UserRepository>();

            services.AddScoped<IAuthService , AuthService>();
            services.AddTransient<IPasswordHasher , PasswordHasher>();
            services.AddTransient<AuthController>();

            return services.BuildServiceProvider();
        }
    }
}
