using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using RandomizedWitchNobeta.Behaviours;
using RandomizedWitchNobeta.Config;
using RandomizedWitchNobeta.Features;
using RandomizedWitchNobeta.Features.Timer;
using RandomizedWitchNobeta.Utils;
using UnityEngine;

namespace RandomizedWitchNobeta;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("LittleWitchNobeta")]
public class Plugin : BasePlugin
{
    internal new static ManualLogSource Log;

    public static DirectoryInfo ConfigDirectory;
    public static DirectoryInfo PluginInstallationDirectory;
    public static ConfigFile ConfigFile;

    private static Harmony _harmony;

    private static AutoConfigManager AutoConfigManager;

    public override void Load()
    {
        Log = base.Log;
        Log.LogMessage($"Plugin {MyPluginInfo.PLUGIN_GUID} is loading...");

        // Fix ImGUI task preventing the game from closing
        Application.quitting += (Action) (() =>
        {
            Singletons.SettingsService.Stop();
            Unload();
        });

        // Plugin startup logic
        ConfigDirectory = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(Config.ConfigFilePath)!, "RandomizedWitchNobeta"));
        ConfigDirectory.Create();

        PluginInstallationDirectory = new DirectoryInfo(Path.Combine(ConfigDirectory.FullName, "../../plugins/RandomizedWitchNobeta"));

        ConfigFile = new ConfigFile(Path.Combine(ConfigDirectory.FullName, "RandomizedWitchNobeta.cfg"), true, GetType().GetCustomAttribute<BepInPlugin>());

        AutoConfigManager = new AutoConfigManager(ConfigFile);
        AutoConfigManager.LoadValuesToFields();

        // Fetch Nobeta process early to get game window handle
        NobetaProcessUtils.NobetaProcess = Process.GetProcessesByName("LittleWitchNobeta")[0];
        NobetaProcessUtils.GameWindowHandle = NobetaProcessUtils.FindWindow(null, "Little Witch Nobeta");

        // Load settings web app process
        Singletons.SettingsService = new SettingsService();
        new Thread(_ => Task.Run(Singletons.SettingsService.Run))
        {
            IsBackground = true
        }.Start();

        // Apply patches
        ApplyPatches();

        // Add required Components
        AddComponent<UnityMainThreadDispatcher>();
        Singletons.Timers = AddComponent<Timers>();

        Log.LogMessage($"Plugin {MyPluginInfo.PLUGIN_GUID} successfully loaded!");
    }

    public override bool Unload()
    {
        Log.LogMessage($"Plugin {MyPluginInfo.PLUGIN_GUID} unloading...");

        SaveConfigs();

        Log.LogMessage($"Plugin {MyPluginInfo.PLUGIN_GUID} successfully unloaded");

        return false;
    }

    public static void SaveConfigs()
    {
        Log.LogInfo("Saving configs...");

        // Save BepInEx config
        AutoConfigManager.FetchValuesFromFields();
        ConfigFile.Save();

        // Save current run
        Singletons.RuntimeVariables?.Save();

        Log.LogInfo("Configs saved");
    }

    public static void ApplyPatches()
    {
        _harmony = new Harmony(nameof(RandomizedWitchNobeta));

        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            if (methods.Any(method => method.GetCustomAttribute<HarmonyPatch>() is not null))
            {
                _harmony.PatchAll(type);
            }
        }
    }
}