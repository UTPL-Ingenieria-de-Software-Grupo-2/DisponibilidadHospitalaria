using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisponibilidadHospitalaria
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .MinimumLevel.Information()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                        .MinimumLevel.Override("Microsoft.Hosting", LogEventLevel.Information)
                        .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Month, rollOnFileSizeLimit: true, fileSizeLimitBytes: 10485760, retainedFileCountLimit: 60)
                        .WriteTo.Console()
                        .CreateLogger();

            try
            {
                Log.Information("Starting up");

                CreateHostBuilder(args)
                    .Build()
                    .DatabaseMigrate()
                    .DatabaseInitialize()
                    .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog();
                });
    }
}
