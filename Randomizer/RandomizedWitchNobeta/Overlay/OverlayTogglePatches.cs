using HarmonyLib;

namespace RandomizedWitchNobeta.Overlay;

public static class OverlayTogglePatches
{
    [HarmonyPatch(typeof(Game), nameof(Game.SwitchScene))]
    [HarmonyPrefix]
    private static void SwitchScenePrefix(SceneSwitchData sceneData, float fadeInDuration)
    {
        OverlayState.ShowOverlay = false;
    }

    [HarmonyPatch(typeof(UIOpeningMenu), nameof(UIOpeningMenu.Init))]
    [HarmonyPostfix]
    private static void OpeningMenuInitPostfix(UIOpeningMenu __instance)
    {
        OverlayState.ShowOverlay = true;
    }
}