using HarmonyLib;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Features.EndConditions;

public static class AllChestEndConditionPatches
{
    [HarmonyPatch(typeof(TreasureBox), nameof(TreasureBox.SetOpen))]
    [HarmonyPostfix]
    private static void OpenPostfix(TreasureBox __instance)
    {
        if (Singletons.RuntimeVariables is { } runtimeVariables)
        {
            runtimeVariables.OpenedChests.Add(__instance.name);
        }
    }
}