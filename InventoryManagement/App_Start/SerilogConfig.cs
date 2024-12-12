using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace InventoryManagement.App_Start
{
    public class SerilogConfig
    {
        public static void ConfigureLoggin()
        {
            var logFilePath = HostingEnvironment.MapPath("~/logs/log-.txt");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Serilog in configured for loggin in file");
        }
    }
}