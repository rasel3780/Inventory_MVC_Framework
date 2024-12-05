using Autofac;
using Autofac.Integration.Mvc;
using InventoryManagement.DbContexts;
using System;
using System.Collections.Generic;
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
            builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}