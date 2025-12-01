using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using nfl_api.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        // Read database configuration from environment variables
        var pgHost = Environment.GetEnvironmentVariable("PG_HOST") ?? throw new InvalidOperationException("PG_HOST environment variable is required");
        var pgUser = Environment.GetEnvironmentVariable("PG_USER") ?? throw new InvalidOperationException("PG_USER environment variable is required");
        var pgPassword = Environment.GetEnvironmentVariable("PG_PASSWORD") ?? throw new InvalidOperationException("PG_PASSWORD environment variable is required");
        var pgDatabase = Environment.GetEnvironmentVariable("PG_DATABASE") ?? throw new InvalidOperationException("PG_DATABASE environment variable is required");

        // Register the NFL schedule service
        services.AddSingleton<INflScheduleService>(sp => 
            new NflScheduleService(pgHost, pgUser, pgPassword, pgDatabase));
    })
    .Build();

host.Run();
