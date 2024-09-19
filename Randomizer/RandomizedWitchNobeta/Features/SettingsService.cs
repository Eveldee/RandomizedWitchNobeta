using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RandomizedWitchNobeta.Config.Serialization;
using RandomizedWitchNobeta.Features.Bonus;
using RandomizedWitchNobeta.Features.UI;
using RandomizedWitchNobeta.Shared;
using UnityEngine;

namespace RandomizedWitchNobeta.Features;

public sealed class SettingsService
{
    public event Action<SeedSettings> SeedSettingsUpdated;

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
            Arguments = $"\"{StartPatches.SeedSettingsPath}\" \"{Environment.ProcessId}\"",
            WorkingDirectory = Path.GetDirectoryName(ServerExecutablePath)!
        });

        Plugin.Log.LogInfo("Settings server started, starting read loop...");

        // Wait until exit
        await Task.Delay(-1, cancellationToken);

        Plugin.Log.LogInfo("Stopping settings server...");

        serverProcess?.Close();
    }

    public void ReloadBonusSettings(BonusSettings bonusSettings = null)
    {
        if (bonusSettings is null)
        {
            try
            {
                bonusSettings =
                    SerializeUtils.Deserialize<BonusSettings>(File.ReadAllText(StartPatches.BonusSettingsPath));
            }
            catch (Exception exception)
            {
                Plugin.Log.LogError("Could not read bonus settings!");
                Plugin.Log.LogError(exception);

                return;
            }
        }

        AppearancePatches.SelectedSkin = (GameSkin) bonusSettings.SelectedSkin;
        AppearancePatches.RandomizeSkin = bonusSettings.RandomizeSkin;
        AppearancePatches.HideBagEnabled = bonusSettings.HideBag;
        AppearancePatches.HideStaffEnabled = bonusSettings.HideStaff;
        AppearancePatches.HideHatEnabled = bonusSettings.HideHat;
    }

    private async void WatcherOnChanged(object sender, FileSystemEventArgs e)
    {
        // Ignore changes to other files
        if (e.ChangeType is not (WatcherChangeTypes.Created or WatcherChangeTypes.Changed)
            || (e.FullPath != StartPatches.SeedSettingsPath && e.FullPath != StartPatches.BonusSettingsPath))
        {
            return;
        }

        await Task.Delay(100);

        // Wait for file to be correctly written
        string fileContent;
        while (true)
        {
            try
            {
                fileContent = await File.ReadAllTextAsync(e.FullPath);

                break;
            }
            catch (Exception)
            {
                await Task.Delay(100);
            }
        }

        // Seed settings
        if (e.FullPath == StartPatches.SeedSettingsPath)
        {
            var settings = SerializeUtils.Deserialize<SeedSettings>(fileContent);

            if (settings is null)
            {
                Plugin.Log.LogError($"Incorrect settings detected at: '{e.FullPath}'");

                return;
            }

            Plugin.Log.LogDebug("Seed settings changed");

            SeedSettingsUpdated?.Invoke(settings);
        }

        // Bonus Settings
        if (e.FullPath == StartPatches.BonusSettingsPath && SerializeUtils.Deserialize<BonusSettings>(fileContent) is { } bonusSettings)
        {
            ReloadBonusSettings(bonusSettings);
        }
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();
    }
}