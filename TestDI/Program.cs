using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TestDI.Services.Interfaces;

namespace TestDI;

class Program
{
    static void Main(string[] args)
    {
        IServiceCollection services = new ServiceCollection();

        Startup startup = new Startup();
        startup.ConfigureServices(services);

        using var serviceProvider = services.BuildServiceProvider();


        var logger = serviceProvider.GetService<ILoggerFactory>()!.CreateLogger<Program>();
        //logger.LogDebug("Logger is working!");


        // Get Service and call method
        var service = serviceProvider.GetService<IMyService>();

        service.MyServiceMethod().Wait();
        
    }
}