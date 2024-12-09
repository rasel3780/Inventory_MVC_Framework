using Autofac;
using Autofac.Integration.Mvc;
using InventoryManagement.DbContexts;
using InventoryManagement.Models;
using InventoryManagement.Repositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.App_Start
{
    public class AutofacConfig
    {
        public static void RegisterDependencies ()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.Register(c=>
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

            builder.RegisterType<ProductRepository>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<OrderRepository>()
                .AsSelf()
                .InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}