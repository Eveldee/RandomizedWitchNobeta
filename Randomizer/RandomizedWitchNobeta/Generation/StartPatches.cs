using System;
using HarmonyLib;
using MarsSDK;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace RandomizedWitchNobeta.Generation;

public static class StartPatches
{
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

        // TODO Start generation then generate and load save
        var generator = new SeedGenerator(new GeneratorSettings { Seed = 42 });
        generator.Generate();

        return false;
    }
}