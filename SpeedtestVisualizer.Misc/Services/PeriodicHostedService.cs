using System.Diagnostics;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpeedtestVisualizer.Misc.Constants;
using SpeedtestVisualizer.Misc.Contexts;
using SpeedtestVisualizer.Misc.Objects;

namespace SpeedtestVisualizer.Misc.Services;

public class PeriodicHostedService : BackgroundService
{
    private readonly TimeSpan _period;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PeriodicHostedService> _logger;
    private readonly IServiceScopeFactory _factory;
    private int _executionCount;

    public PeriodicHostedService(
        IConfiguration configuration,
        ILogger<PeriodicHostedService> logger, 
        IServiceScopeFactory factory)
    {
        _configuration = configuration;
        _logger = logger;
        _factory = factory;
        _period = TimeSpan.FromMinutes(int.Parse(_configuration[AppSettingConstants.SpeedtestTestingConfigurationInterval]));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_period);
        while (
            !stoppingToken.IsCancellationRequested &&
            await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                SpeedtestVisualizerContext context = asyncScope.ServiceProvider.GetRequiredService<SpeedtestVisualizerContext>();
                
                var watch = new Stopwatch();

                HttpResponseMessage response;
                using (var client = new HttpClient())
                {
                    var httpRequest = new HttpRequestMessage();
                    httpRequest.Headers.Add(HttpRequestHeader.UserAgent.ToString(), "SpeedtestVisualizer (version 0.1.0");
                    httpRequest.Method = HttpMethod.Get;
                    httpRequest.RequestUri = new Uri(_configuration[AppSettingConstants.SpeedtestTestingConfigurationDownloadUri]);
                    watch.Start();
                    response = await client.SendAsync(httpRequest);
                    watch.Stop();
                }
                var downstreamSpeed = (await response.Content.ReadAsByteArrayAsync()).LongLength / watch.Elapsed.TotalSeconds;
                
                watch = new Stopwatch();
                
                var bytes = new Byte[int.Parse(_configuration[AppSettingConstants.SpeedtestTestingConfigurationUploadByteArraySize])];
                new Random().NextBytes(bytes);
                using (var client = new HttpClient())
                {
                    var httpRequest = new HttpRequestMessage();
                    httpRequest.Headers.Add(HttpRequestHeader.UserAgent.ToString(), "SpeedtestVisualizer (version 0.1.0");
                    httpRequest.Method = HttpMethod.Post;
                    httpRequest.RequestUri = new Uri(_configuration[AppSettingConstants.SpeedtestTestingConfigurationUploadUri]);
                    httpRequest.Content = new ByteArrayContent(bytes);
                    watch.Start();
                    _ = await client.SendAsync(httpRequest);
                    watch.Stop();
                }
                var upstreamSpeed = bytes.LongLength / watch.Elapsed.TotalSeconds;
            
                context.SpeedtestResults.Add(new SpeedtestResult(DateTime.Now, (Int64)downstreamSpeed, (Int64)upstreamSpeed));
                await context.SaveChangesAsync();
                
                _executionCount++;
                _logger.LogInformation(
                    $"Executed PeriodicHostedService - Count: {_executionCount}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(
                    $"Failed to execute PeriodicHostedService with exception message {ex.Message}. Good luck next round!");
            }
        }
    }
}