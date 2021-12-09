using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using Polly.Wrap;
using TestDI.Services.Interfaces;

namespace TestDI.Services;

internal class PollyService : IPollyService
{
    private readonly ILogger<PollyService> _logger;

    public PollyService(ILogger<PollyService> logger)
    {
        _logger = logger;
    }

    public async Task TestPolly()
    {
        // await Test01();
        await Test02();

            

        _logger.LogInformation("Finished");

        await Task.CompletedTask;
    }

    private async Task Test01()
    {
        int counter = 0;
        Policy
            .Handle<Exception>()
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(1))
            .Execute(() =>
            {
                _logger.LogDebug($"{++counter}");
                if (counter != 3)
                    throw new InvalidTimeZoneException($"{counter} attempt");
            });

        await Task.CompletedTask;
    }

    private async Task Test02()
    {
        int counter = 0;

        Action<Exception, TimeSpan> onBreak = (exception, timespan) => { _logger.LogWarning("Circuit broken!"); };
        Action onReset = () => { _logger.LogWarning("Circuit Reset!"); };

        AsyncRetryPolicy retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(2, _ => TimeSpan.FromSeconds(0.2));

        AsyncCircuitBreakerPolicy breaker = Policy
            .Handle<InvalidTimeZoneException>()
            .CircuitBreakerAsync(
                3,
                TimeSpan.FromSeconds(0.6),
                onBreak,
                onReset
            );

        AsyncTimeoutPolicy timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(5), TimeoutStrategy.Pessimistic);

        var wrap = Policy.WrapAsync(timeoutPolicy, retryPolicy, breaker);

        // breaker.Isolate();

        async Task MyExecute(bool isThrow = false, bool isForce = false)
        {
            try
            {
                Console.WriteLine($"{++counter} - {breaker.CircuitState}");
                await breaker.ExecuteAsync(async () =>
                {
                    await Task.Delay(10000);
                    Console.Write("-");
                    if (isThrow)
                        throw new InvalidTimeZoneException($"{counter} attempt");
                });
                Console.WriteLine("Good");
            }
            catch (InvalidTimeZoneException)
            {
                // StringBuilder mes = new();
                // foreach (var number in Enumerable.Range(0, 10))
                // {
                //     Thread.Sleep(100);
                //     mes.AppendLine($"Throw breaker - {breaker.CircuitState}");
                // }
                // _logger.LogError(mes.ToString());
                if (breaker.CircuitState == CircuitState.Closed || isForce)
                    Console.WriteLine($"Throw breaker - {breaker.CircuitState}");
                else
                    foreach (var number in Enumerable.Range(0, 10))
                    {
                        Thread.Sleep(100);
                        Console.WriteLine($"Throw breaker - {breaker.CircuitState}");
                    }
            }
            catch (BrokenCircuitException)
            {
                Console.WriteLine($"BrokenCircuitException - {breaker.CircuitState}");
            }
            catch (TimeoutRejectedException)
            {
                Console.WriteLine($"TimeoutRejectedException - {breaker.CircuitState}");
            }

        }

        await MyExecute();
        await Task.Delay(1000);

        await MyExecute(true);
        await Task.Delay(1000);
        await MyExecute(true);
        await Task.Delay(1000);
        await MyExecute(true);
        await Task.Delay(1000);

        await MyExecute(true, true);
        await MyExecute();
        await MyExecute();
        await Task.Delay(1000);

        await MyExecute(true);
        await MyExecute(true, true);
        await Task.Delay(1000);

        await MyExecute(true);
        await Task.Delay(1000);

        await MyExecute();
        await MyExecute();
        await MyExecute();
        await Task.Delay(1000);

        await MyExecute(true);
        await MyExecute(true);

        await Task.Delay(1000);

        await MyExecute();

        await MyExecute(true);
    }

    private async Task Test03()
    {

    }
}