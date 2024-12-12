using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using InventoryManagement.App_Start;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace InventoryManagement
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Autofac
            AutofacConfig.RegisterDependencies();
            
            //Serilog
            SerilogConfig.ConfigureLoggin();
          
            Log.Information("Application Starting");

        }
       
    }
}
