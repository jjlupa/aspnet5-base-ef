using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace aspnet5_base_ef
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .CaptureStartupErrors(true)
                    .UseSerilog((hostingContext, loggerConfiguration) =>
                    {
                        loggerConfiguration
                            .ReadFrom.Configuration(hostingContext.Configuration)
                            .Enrich.FromLogContext()
                            .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                            .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment);
#if DEBUG
                        // Used to filter out potentially bad data due debugging.
                        loggerConfiguration.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached);
#endif
                    });
                });
    }
}
