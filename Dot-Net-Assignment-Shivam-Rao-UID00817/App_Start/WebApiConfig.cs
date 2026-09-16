using Dot_Net_Assignment_Shivam_Rao_UID00817.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exception_Handlers;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Filters;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi" ,
                routeTemplate: "api/{controller}/{id}" ,
                defaults: new { id = RouteParameter.Optional }
            );

            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());

            config.Filters.Add(new ModelAttributeValidation()); 

            DependencyInjectionConfig.RegisterDependencies();
        }
    }
}
