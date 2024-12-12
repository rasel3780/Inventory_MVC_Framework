using Autofac;
using Autofac.Integration.Mvc;
using InventoryManagement.DbContexts;
using InventoryManagement.Repositories;
using InventoryManagement.Services;
using Serilog;
using System.Configuration;
using System.Reflection;
using System.Web.Mvc;

namespace InventoryManagement.App_Start
{
    public class AutofacConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.Register(c =>
            {
                string connectionString = ConfigurationManager.ConnectionStrings["InventoryConString"].ConnectionString;
                return new ApplicationDbContext(connectionString);
            }).AsSelf().InstancePerRequest();

            //Serilog
            builder.RegisterInstance(Log.Logger).As<ILogger>().SingleInstance();

            //Repositories
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerRequest();

            builder.RegisterType<AccountRepository>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<ReportRepository>()
                .As<IReportRrepository>()
                .InstancePerRequest();

            builder.RegisterType<ProductRepository>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<OrderRepository>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<CustomerRepository>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<VendorRepository>()
                .AsSelf()
                .InstancePerRequest();

            //Services
            builder.RegisterType<AccountService>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<ProductService>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<OrderService>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<ReportService>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<VendorService>()
                .AsSelf()
                .InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}