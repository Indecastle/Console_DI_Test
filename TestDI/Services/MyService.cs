using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TestDI.Options;
using TestDI.Services.Interfaces;

namespace TestDI.Services;

public class MyService : IMyService
{
    private readonly IOptions<CommonWebOptions> _commonWebOptions;
    private readonly ILogger<MyService> _logger;
    private readonly ITestCSharp9Service _testCSharp9Service;
    private readonly ITestCSharp10Service _testCSharp10Service;
    private readonly IPollyService _pollyService;

    private readonly string _baseUrl;
    private readonly string _token;

    public MyService(
        IConfigurationRoot config,
        IOptions<CommonWebOptions> commonWebOptions,
        ILogger<MyService> logger,
        ITestCSharp9Service testCSharp9Service,
        ITestCSharp10Service testCSharp10Service,
        IPollyService pollyService)
    {
        _commonWebOptions = commonWebOptions;
        _logger = logger;
        _testCSharp9Service = testCSharp9Service;
        _testCSharp10Service = testCSharp10Service;
        _pollyService = pollyService;
        var baseUrl = _commonWebOptions.Value.BaseUrl;
        var token = _commonWebOptions.Value.Token;
        //var baseUrl = config["CommonWeb:BaseUrl"];
        //var token = config["CommonWeb:Token"];

        _baseUrl = baseUrl;
        _token = token;
    }

    public async Task MyServiceMethod()
    {
        // _logger.LogDebug(_baseUrl);
        // _logger.LogInformation(_token);
        // _logger.LogWarning(_token);
        // _logger.LogError(_token);
        // _logger.LogCritical(_token);
        // throw new InvalidOperationException("test throw");

        // _testCSharp9Service.Test();
        // _testCSharp10Service.Test();

        await _pollyService.TestPolly();

        await Task.CompletedTask;
    }
}