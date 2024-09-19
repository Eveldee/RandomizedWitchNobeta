using HarmonyLib;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Features.Timer;

public static class TimersPatches
{
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