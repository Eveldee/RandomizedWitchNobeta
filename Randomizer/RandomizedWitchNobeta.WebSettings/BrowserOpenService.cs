using System.Diagnostics;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace RandomizedWitchNobeta.WebSettings;

public class BrowserOpenService : BackgroundService
{
    private readonly ILogger<BrowserOpenService> _logger;
    private readonly IServer _server;

    public BrowserOpenService(ILogger<BrowserOpenService> logger, IServer server)
    {
        _logger = logger;
        _server = server;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Open browser at url
        _logger.LogInformation("Opening browser at index...");

        // Wait for server to start to get address
        await Task.Delay(2000, stoppingToken);

        var addressesFeature = _server.Features.Get<IServerAddressesFeature>();

        if (addressesFeature?.Addresses is { Count: >= 1 } addresses)
        {
            var url = addresses.First();

            _logger.LogInformation("Opening browser at {Url}", url);

            Process.Start(new ProcessStartInfo { FileName = $"{url}/index.html", UseShellExecute = true });
        }
        else
        {
            _logger.LogError("No url found, can't open browser");
        }
    }
}