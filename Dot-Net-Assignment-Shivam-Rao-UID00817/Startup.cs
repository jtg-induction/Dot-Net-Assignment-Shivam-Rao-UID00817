using Dot_Net_Assignment_Shivam_Rao_UID00817.Middlewares;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Owin;

[assembly: OwinStartup(typeof(Dot_Net_Assignment_Shivam_Rao_UID00817.Startup))]
namespace Dot_Net_Assignment_Shivam_Rao_UID00817
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use<AuthenticationMiddleware>();
            app.UseStageMarker(PipelineStage.Authenticate);
        }
    }
}
