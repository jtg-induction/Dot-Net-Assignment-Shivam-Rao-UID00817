using Dot_Net_Assignment_Shivam_Rao_UID00817.App_Start;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exception_Handlers;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Filters;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Services.Replace(typeof(IExceptionHandler) , new GlobalExceptionHandler());

            config.Filters.Add(new ModelAttributeValidation());

            DependencyInjectionConfig.RegisterDependencies();
        }
    }
}
