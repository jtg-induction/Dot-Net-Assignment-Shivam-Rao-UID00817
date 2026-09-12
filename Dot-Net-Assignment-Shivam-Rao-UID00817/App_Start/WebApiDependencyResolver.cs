using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Web;
using System.Web.Http.Dependencies;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.App_Start
{
    public class WebApiDependencyResolver: IDependencyResolver
    {
        private readonly IServiceScope _scope;
        private readonly IServiceProvider _serviceProvider;

        public WebApiDependencyResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private WebApiDependencyResolver(IServiceScope scope)
        {
            _scope = scope;
            _serviceProvider = scope.ServiceProvider;
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _serviceProvider.GetServices(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            IServiceScope scope = _serviceProvider.CreateScope();

            return new WebApiDependencyResolver(scope);
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}
