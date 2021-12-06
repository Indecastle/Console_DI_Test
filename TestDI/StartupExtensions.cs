using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TestDI.Options;

namespace TestDI
{
    internal static class StartupExtensions
    {
        public static void AddOptions(IServiceCollection services, IConfigurationRoot configuration)
        {
            //configuration.GetSection(CommonWebOptions.Position).Bind(new CommonWebOptions());
            services.Configure<CommonWebOptions>(configuration.GetSection(CommonWebOptions.Position));
        }
    }
}
