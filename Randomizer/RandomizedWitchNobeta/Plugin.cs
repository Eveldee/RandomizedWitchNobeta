using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using RandomizedWitchNobeta.Behaviours;
using RandomizedWitchNobeta.Bonus;
using RandomizedWitchNobeta.Config;
using RandomizedWitchNobeta.Generation;
using RandomizedWitchNobeta.Overlay;
using RandomizedWitchNobeta.Patches.Gameplay;
using RandomizedWitchNobeta.Patches.Shuffle;
using RandomizedWitchNobeta.Patches.UI;
using RandomizedWitchNobeta.Timer;
using RandomizedWitchNobeta.Utils;
using UnityEngine;

namespace RandomizedWitchNobeta;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("LittleWitchNobeta")]
public class Plugin : BasePlugin
{
    internal new static ManualLogSource Log;

    public static NobetaRandomizerOverlay NobetaRandomizerOverlay = null;
    public static DirectoryInfo ConfigDirectory;
    public static ConfigFile ConfigFile;

    private static AutoConfigManager AutoConfigManager;

    public override void Load()
    {
        Log = base.Log;
        Log.LogMessage($"Plugin {MyPluginInfo.PLUGIN_GUID} is loading...");

        // Fix ImGUI task preventing the game from closing
        Application.quitting += (Action) (() =>
        {
            NobetaRandomizerOverlay?.Close();
            Unload();
        });

        // Plugin startup logic
        ConfigDirectory = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(Config.ConfigFilePath)!, "RandomizedWitchNobeta"));
        ConfigDirectory.Create();
        ConfigFile = new ConfigFile(Path.Combine(ConfigDirectory.FullName, "RandomizedWitchNobeta.cfg"), true, GetType().GetCustomAttribute<BepInPlugin>());

        AutoConfigManager = new AutoConfigManager(ConfigFile);
        AutoConfigManager.LoadValuesToFields();

        // Fetch Nobeta process early to get game window handle
        NobetaProcessUtils.NobetaProcess = Process.GetProcessesByName("LittleWitchNobeta")[0];
        NobetaProcessUtils.GameWindowHandle = NobetaProcessUtils.FindWindow(null, "Little Witch Nobeta");

        // Create and show overlay
        #if !NOUI
        NobetaRandomizerOverlay = new NobetaRandomizerOverlay();
        Task.Run(NobetaRandomizerOverlay.Run);
        #endif

        // Apply patches
        ApplyPatches();

        // Add required Components
        AddComponent<UnityMainThreadDispatcher>();
        Singletons.Timers = AddComponent<Timers>();

        // Load settings web app process


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
        Harmony.CreateAndPatchAll(typeof(Singletons));
        Harmony.CreateAndPatchAll(typeof(SceneUtils));
        Harmony.CreateAndPatchAll(typeof(ConfigPatches));

        Harmony.CreateAndPatchAll(typeof(OverlayTogglePatches));

        Harmony.CreateAndPatchAll(typeof(StartPatches));
        Harmony.CreateAndPatchAll(typeof(RunCompletePatches));

        // Gameplay patches
        Harmony.CreateAndPatchAll(typeof(TeleportMenuPatches));
        Harmony.CreateAndPatchAll(typeof(LockedDoorPatches));
        Harmony.CreateAndPatchAll(typeof(ArcaneDisabledPatches));
        Harmony.CreateAndPatchAll(typeof(MagicUpgradePatches));
        Harmony.CreateAndPatchAll(typeof(ItemPoolSizePatches));
        Harmony.CreateAndPatchAll(typeof(TrialKeysPatches));
        Harmony.CreateAndPatchAll(typeof(EndConditionsPatches));
        Harmony.CreateAndPatchAll(typeof(CombatPatches));

        // UI Patches
        Harmony.CreateAndPatchAll(typeof(StatueUnlockPatches));
        Harmony.CreateAndPatchAll(typeof(GameTipPatches));

        // Randomizer patches
        Harmony.CreateAndPatchAll(typeof(ChestContentShufflePatches));
        Harmony.CreateAndPatchAll(typeof(ExitShufflePatches));
        Harmony.CreateAndPatchAll(typeof(ChestExtraLootPatches));
        Harmony.CreateAndPatchAll(typeof(SpecialLootPatches));

        // Bonus patches
        Harmony.CreateAndPatchAll(typeof(AppearancePatches));

        #if !NOUI
        Harmony.CreateAndPatchAll(typeof(TimersPatches));
        #endif
    }
}