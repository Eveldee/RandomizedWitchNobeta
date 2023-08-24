using System;
using HarmonyLib;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Timer;

public static class TimersPatches
{
    // [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.OnSceneInitComplete))]
    // [HarmonyPostfix]
    // private static void OnSceneInitCompletePostfix()
    // {
    //     Singletons.Timers.Resume();
    // }
    //
    // [HarmonyPatch(typeof(Game), nameof(Game.EnterLoaderScene))]
    // [HarmonyPrefix]
    // private static void EnterLoaderScenePrefix()
    // {
    //     Singletons.Timers.Pause();
    // }

    [HarmonyPatch(typeof(Game), nameof(Game.SwitchTitleScene))]
    [HarmonyPrefix]
    private static void OSwitchTitleScenePrefix()
    {
        if (Singletons.Timers is { } timers)
        {
            timers.Pause();
        }
    }

    [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.OnSceneInitComplete))]
    [HarmonyPostfix]
    private static void OnSceneInitCompletePostfix()
    {
        if (Singletons.Timers is { } timers)
        {
            timers.Resume();
        }
    }
}