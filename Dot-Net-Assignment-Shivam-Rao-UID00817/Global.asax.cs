using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using dotenv.net;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            string envPath = AppDomain.CurrentDomain.BaseDirectory + ".env";
            DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { envPath }));
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
