using Autofac;
using Autofac.Integration.WebApi;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using System.Reflection;
using System.Web.Http;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.App_Start
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<Restaurant_ManagementContext>().InstancePerRequest();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();

            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest();

            builder.RegisterType<RefreshTokenRepository>().As<IRefreshTokenRepository>().InstancePerRequest();

            builder.RegisterType<RestaurantRepository>().As<IRestaurantRepository>().InstancePerRequest();

            builder.RegisterType<OwnerManagesRestaurantsRepository>().As<IOwnerManagesRestaurantsRepository>().InstancePerRequest();

            builder.RegisterType<ItemRepository>().As<IItemsRepository>().InstancePerRequest();

            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerRequest();

            builder.RegisterType<AddressRepository>().As<IAddressRepository>().InstancePerRequest();

            builder.RegisterType<AuthService>().As<IAuthService>().InstancePerRequest();

            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();

            builder.RegisterType<AdminRestaurantService>().As<IAdminRestaurantService>().InstancePerRequest();

            builder.RegisterType<BrowseService>().As<IBrowseService>().InstancePerRequest();

            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerRequest();

            var container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
