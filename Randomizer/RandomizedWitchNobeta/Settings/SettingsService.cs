using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RandomizedWitchNobeta.Config.Serialization;
using RandomizedWitchNobeta.Generation;
using RandomizedWitchNobeta.Patches.UI;
using RandomizedWitchNobeta.Shared;
using UnityEngine;

namespace RandomizedWitchNobeta.Settings;

public sealed class SettingsService
{
    public event Action<SeedSettings> SettingsUpdated;

    private static string ServerExecutablePath { get; } = Path.Combine(Plugin.PluginInstallationDirectory.FullName, "RandomizedWitchNobeta.WebSettings.exe");

    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private FileSystemWatcher _watcher;

    public async Task Run()
    {
        var cancellationToken = _cancellationTokenSource.Token;

        // Check that server executable exists
        if (!File.Exists(ServerExecutablePath))
        {
            Plugin.Log.LogError($"Missing settings server executable at path: '{ServerExecutablePath}', exiting...");

            Application.Quit();
        }

        // Setup file system watcher
        _watcher = new FileSystemWatcher(Path.GetDirectoryName(StartPatches.SeedSettingsPath)!, "*.json")
        {
            IncludeSubdirectories = false,
            EnableRaisingEvents = true
        };

        _watcher.Created += WatcherOnChanged;
        _watcher.Changed += WatcherOnChanged;

        // Create web api process and pass settings path
        var serverProcess = Process.Start(new ProcessStartInfo(ServerExecutablePath)
        {
            Arguments = StartPatches.SeedSettingsPath,
            WorkingDirectory = Path.GetDirectoryName(ServerExecutablePath)!,
            UseShellExecute = true
        });

        Plugin.Log.LogInfo("Settings server started, starting read loop...");

        // Wait until exit
        await Task.Delay(-1, cancellationToken);

        Plugin.Log.LogInfo("Stopping settings server...");

        serverProcess?.Close();
    }

    private void WatcherOnChanged(object sender, FileSystemEventArgs e)
    {
        // Ignore changes to other files
        if (e.ChangeType is not (WatcherChangeTypes.Created or WatcherChangeTypes.Changed) || e.FullPath != StartPatches.SeedSettingsPath)
        {
            return;
        }

        var settings = SerializeUtils.Deserialize<SeedSettings>(File.ReadAllText(StartPatches.SeedSettingsPath));

        if (settings is null)
        {
            Plugin.Log.LogError($"Incorrect settings detected at: '{e.FullPath}'");

            return;
        }

        Plugin.Log.LogDebug("Seed settings changed");

        SettingsUpdated?.Invoke(settings);
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();
    }
}