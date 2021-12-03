using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Api
{
    public static class Program
    {
        private static readonly string LogFilePath =  "nlog.config";
        
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog(LogFilePath).GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureLogging(
                    logging =>
                    {
                        logging.AddConsole();
                        logging.AddDebug();
                        logging.ClearProviders();
                        logging.SetMinimumLevel(LogLevel.Debug);
                    })
                .UseNLog();
    }
}