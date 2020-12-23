using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Logging
{
    public static class Extensions
    {
        public static IHostBuilder UseCustomSerilog(this IHostBuilder builder)
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



// nuget push SerilogLogging.1.2.0.nupkg oy2ln3eyzm7ikjepf27azzvvvlupudeelobalukmzhqh3i -Source https://api.nuget.org/v3/index.json