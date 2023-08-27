using System;
using System.IO;
using HarmonyLib;
using MarsSDK;
using RandomizedWitchNobeta.Bonus;
using RandomizedWitchNobeta.Config.Serialization;
using RandomizedWitchNobeta.Overlay;
using RandomizedWitchNobeta.Runtime;
using RandomizedWitchNobeta.Utils;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace RandomizedWitchNobeta.Generation;

public static class StartPatches
{
    public const int GameSaveIndex = 9;

    public static Text CopyrightText { get; private set; }

    [HarmonyPatch(typeof(UIOpeningMenu), nameof(UIOpeningMenu.Init))]
    [HarmonyPostfix]
    private static void OpeningMenuInitPostfix(UIOpeningMenu __instance)
    {
        var newGameObject = __instance.transform.Find("Foreground/NewGame").gameObject;

        if (newGameObject != null)
        {
            var newGameUIHandler = newGameObject.GetComponent<UILabelHandler>();
            var newGameLabel = newGameObject.GetComponentInChildren<Text>();

            var loadGameObject = __instance.transform.Find("Foreground/LoadGame").gameObject;
            var optionsUIHandler = __instance.transform.Find("Foreground/Option").gameObject.GetComponent<UILabelHandler>();

            Plugin.Log.LogDebug("Found 'New Game' button");

            newGameLabel.text = "New Randomizer";

            if (RuntimeVariables.TryLoad(out var runtimeVariables))
            {
                loadGameObject.GetComponentInChildren<Text>().text = "Resume Run";
                Singletons.RuntimeVariables = runtimeVariables;
            }
            // Remove Load button if no run is resume-able
            else
            {
                Object.Destroy(loadGameObject);

                // Reorder UI elements
                newGameUIHandler.selectDown = optionsUIHandler;
                optionsUIHandler.selectUp = newGameUIHandler;
            }

            // Replace copyright text to add seed hash
            var copyrightGameObject = __instance.transform.Find("Foreground/Copyright").gameObject;
            copyrightGameObject.transform.Translate(0, 5, 0);

            CopyrightText = copyrightGameObject.GetComponent<Text>();
        }
        else
        {
            Plugin.Log.LogError("Couldn't find 'New Game' button, check that you are using the correct game and randomizer version!");
            Application.Quit();
        }
    }

    [HarmonyPatch(typeof(UIOpeningMenu), nameof(UIOpeningMenu.Appear))]
    [HarmonyPostfix]
    private static void OpeningMenuAppearPostfix(UIOpeningMenu __instance)
    {
        __instance.navigator.DeselectHandler(__instance.handlers[1]);
        __instance.OnHandlerDeselected(__instance.handlers[1]);

        __instance.navigator.currentHandler = null;

        __instance.navigator.lastSelectedHandler = Singletons.RuntimeVariables is null
            ? __instance.handlers[2]
            : __instance.handlers[1];
    }

    [HarmonyPatch(typeof(UIGameSave), nameof(UIGameSave.Appear))]
    [HarmonyPrefix]
    private static bool UIGameSaveAppearPrefix(UIGameSave __instance, Action completeHandler)
    {
        Plugin.Log.LogDebug("UIGameSave Appear");

        __instance.gameObject.SetActive(false);
        Game.FadeInBlackScreen(5f);

        if (__instance.pageMode == GameSavePageMode.NewGame)
        {
            StartRandomizer();
        }
        else
        {
            ResumeRandomizer();
        }

        return false;
    }

    private static void StartRandomizer()
    {
        // Generate a seed
        #if NOUI

        SeedSettings settings;
        var settingsPath = Path.Combine(Plugin.ConfigDirectory.FullName, "SeedSettings.json");

        if (!File.Exists(settingsPath))
        {
            settings = new SeedSettings();
            File.WriteAllText(settingsPath, SerializeUtils.SerializeIndented(settings));
        }
        else
        {
            settings = SerializeUtils.Deserialize<SeedSettings>(File.ReadAllText(settingsPath));
        }

        #else

        var settings = OverlayState.SeedSettings;

        #endif

        var generator = new SeedGenerator(settings);
        generator.Generate();

        var runtimeVariables = Singletons.RuntimeVariables;

        Plugin.Log.LogMessage("Creating save...");

        // Generate the save and apply flag modifications
        var gameSave = new GameSave(GameSaveIndex, settings.Difficulty)
        {
            basic =
            {
                showTeleportMenu = true,
                // Set save point to avoid "Return to Statue" early
                stage = SceneUtils.SceneNumberToGameStage(runtimeVariables.StartScene),
                savePoint = SceneUtils.SceneStartSavePoint(runtimeVariables.StartScene)
            },
            flags =
            {
                stage01OpenDoor01 = true,
                stage01Room08Door = true,
                stage02L03BackDoor = true
            },
            stats =
            {
                // Give absorb and wind 1, remove Arcane
                secretMagicLevel = settings.NoArcane ? 0 : 1,
                windMagicLevel = 1,
                manaAbsorbLevel = 1,
                // Give souls scaling on start level
                currentMoney = SceneUtils.SceneStartSouls(runtimeVariables.StartScene) * settings.StartSoulsModifier
            }
        };

        Game.WriteGameSave(gameSave);

        Plugin.Log.LogMessage("Save created, loading the game...");

        // Load a random skin if activated
        if (AppearancePatches.RandomizeSkin == AppearancePatches.RandomizeSkin_Once)
        {
            Game.Collection.UpdateSkin((GameSkin)Random.Shared.Next(0, AppearancePatches.AvailableSkins.Length));
        }

        // Load save
        var switchData = new SceneSwitchData(runtimeVariables.StartScene, SceneUtils.SceneStartSavePoint(runtimeVariables.StartScene), true);

        Game.SwitchGameSave(gameSave);
        Game.SwitchScene(switchData);

        Singletons.Timers.Reset();
    }

    private static void ResumeRandomizer()
    {
        if (Game.ReadGameSave(GameSaveIndex, out var gameSave) == ReadFileResult.Succeed)
        {
            Game.SwitchGameSave(gameSave);
            Game.SwitchScene(new SceneSwitchData(gameSave.basic.stage, gameSave.basic.savePoint, false));
        }
        else
        {
            Plugin.Log.LogFatal($"Couldn't load save slot {GameSaveIndex}, quitting...");
            Application.Quit();
        }
    }
}