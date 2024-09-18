using System.Diagnostics;

namespace RandomizedWitchNobeta.WebSettings;

public class AutoCloseService : BackgroundService
{
    private readonly IHostApplicationLifetime _lifetime;
    private readonly int? _parentProcessId;

    public AutoCloseService(IHostApplicationLifetime lifetime, int? parentProcessId)
    {
        _lifetime = lifetime;
        _parentProcessId = parentProcessId;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_parentProcessId is not { } id)
        {
            return;
        }

        try
        {
            var parentProcess = Process.GetProcessById(id);

            await parentProcess.WaitForExitAsync(stoppingToken);
        }
        catch (Exception)
        {
            // If any error occur it means that the process has already exited
        }

        _lifetime.StopApplication();
    }
}