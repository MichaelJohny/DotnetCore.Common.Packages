using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Logging
{
    public static class Extensions
    {
        public static IHostBuilder UseCustomSerilog( IHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json")
                .AddEnvironmentVariables()
                .Build();

            
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            SerilogHostBuilderExtensions.UseSerilog(builder);

            return builder;
            
//            builder.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
//                .ReadFrom.Configuration(configuration)
//                .Enrich.FromLogContext()
//                .WriteTo.Console());
            
        }
    }
}