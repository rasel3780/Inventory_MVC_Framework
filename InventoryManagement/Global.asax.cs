using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
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

            var conString = ConfigurationManager.ConnectionStrings["InventoryConString"].ConnectionString;
            var logFilePath = HostingEnvironment.MapPath("~/logs/log-.txt");
            //Serilog
            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Debug()
                 .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                 .WriteTo.MSSqlServer(
                    connectionString: conString,
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
                    restrictedToMinimumLevel: LogEventLevel.Information
                  )
                 .CreateLogger();

            Log.Information("Application Starting");

        }
       
    }
}
