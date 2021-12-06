using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestDI.Services;
using TestDI.Services.Interfaces;
using TestDI.Options;

namespace TestDI
{
    public class Startup
    {
        IConfigurationRoot Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //configure console logging
            services.AddLogging(opt =>
            {
                opt.SetMinimumLevel(LogLevel.Debug);
                opt.AddConsole();
            });

            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.AddSingleton<IMyService, MyService>();

            StartupExtensions.AddOptions(services, Configuration);
        }
    }
}
