using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TestDI.Options;
using TestDI.Services.Interfaces;

namespace TestDI.Services
{
    public class MyService : IMyService
    {
        private readonly IOptions<CommonWebOptions> _commonWebOptions;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly ILogger<MyService> _logger;

        public MyService(ILoggerFactory loggerFactory, IConfigurationRoot config, IOptions<CommonWebOptions> commonWebOptions, ILogger<MyService> logger)
        {
            _commonWebOptions = commonWebOptions;
            _logger = logger;
            var baseUrl = _commonWebOptions.Value.BaseUrl;
            var token = _commonWebOptions.Value.Token;
            //var baseUrl = config["CommonWeb:BaseUrl"];
            //var token = config["CommonWeb:Token"];

            _baseUrl = baseUrl;
            _token = token;
        }

        public Task MyServiceMethod()
        {
            _logger.LogDebug(_baseUrl);
            _logger.LogInformation(_token);
            _logger.LogWarning(_token);
            _logger.LogError(_token);
            _logger.LogCritical(_token);

            throw new InvalidOperationException("test throw");
            return Task.CompletedTask;
        }
    }
}
