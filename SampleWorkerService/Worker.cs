namespace SampleWorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private HttpClient client;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        client.Dispose();           
        return base.StopAsync(cancellationToken);
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        client = new HttpClient();
        return base.StartAsync(cancellationToken);

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var result = await client.GetAsync("https://www.linkedin.com/in/denizhankalkan/");

            if (result.IsSuccessStatusCode)
            {
                _logger.LogInformation("The website is up. StatusCode:{StatusCode}", result.StatusCode);
            }
            else
            {
                _logger.LogError("The website is down. StatusCode:{StatusCode}", result.StatusCode);

            }
            await Task.Delay(5000, stoppingToken);
        }
    }
}

