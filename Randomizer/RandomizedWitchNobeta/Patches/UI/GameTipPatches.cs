using HarmonyLib;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Patches.UI;

public static class GameTipPatches
{
    [HarmonyPatch(typeof(Game), nameof(Game.AppearGameTip))]
    [HarmonyPrefix]
    // ReSharper disable once RedundantAssignment
    private static void AppearGameTipPrefix(ref GameTipStyle style, ref GameTipStyle __state)
    {
        if (Singletons.RuntimeVariables is not { } runtimeVariables || !runtimeVariables.Settings.GameTips)
        {
            return;
        }

        // Force a specific style to avoid dealing with input icons
        __state = style;

        style = GameTipStyle.SaveGame;
    }

    [HarmonyPatch(typeof(Game), nameof(Game.AppearGameTip))]
    [HarmonyPostfix]
    private static void AppearGameTipPostfix(ref GameTipStyle style, ref GameTipStyle __state)
    {
        if (Singletons.RuntimeVariables is not { } runtimeVariables || !runtimeVariables.Settings.GameTips)
        {
            return;
        }

        // Modify game tip
        var tipUi = Game.stageUI.gameTip;

        tipUi.title.text = "Work in progress";
        tipUi.contentHandler.SetContentData("This feature is in development, it may be available in next release!");
        tipUi.contentHandler.UpdateContentData();
    }
}