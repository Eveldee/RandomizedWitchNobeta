using System;
using HarmonyLib;
using MarsSDK;
using RandomizedWitchNobeta.Utils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace RandomizedWitchNobeta.Generation;

public static class StartPatches
{
    public const int GameSaveIndex = 9;

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

            newGameLabel.text = "Start Randomizer";

            // Remove Load button
            Object.Destroy(loadGameObject);

            // Reorder UI elements
            newGameUIHandler.selectDown = optionsUIHandler;
            optionsUIHandler.selectUp = newGameUIHandler;
        }
        else
        {
            Plugin.Log.LogError("Couldn't find 'New Game' button, check that you are using the correct game and randomizer version!");
            Application.Quit();
        }
    }

    [HarmonyPatch(typeof(UIGameSave), nameof(UIGameSave.Appear))]
    [HarmonyPrefix]
    private static bool UIGameSaveAppearPrefix(UIGameSave __instance, Action completeHandler)
    {
        Plugin.Log.LogDebug("UIGameSave Appear");

        __instance.gameObject.SetActive(false);
        Game.FadeInBlackScreen(5f);

        // Generate a seed
        var generator = new SeedGenerator(new SeedSettings { Seed = 3 });
        generator.Generate();

        var runtimeVariables = Singletons.RuntimeVariables;

        Plugin.Log.LogMessage("Creating save...");

        // Generate the save and apply flag modifications
        var gameSave = new GameSave(GameSaveIndex, GameDifficulty.Advanced)
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
                secretMagicLevel = 0,
                windMagicLevel = 1,
                manaAbsorbLevel = 1
            }
        };

        Game.WriteGameSave(gameSave);

        Plugin.Log.LogMessage("Save created, loading the game...");

        // Load save
        var switchData = new SceneSwitchData(runtimeVariables.StartScene, SceneUtils.SceneStartSavePoint(runtimeVariables.StartScene), true);

        Game.SwitchGameSave(gameSave);
        Game.SwitchScene(switchData);

        return false;
    }
}