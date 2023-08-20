using HarmonyLib;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Runtime;

public static class ChestContentShufflePatches
{
    [HarmonyPatch(typeof(TreasureBox), nameof(TreasureBox.Init))]
    [HarmonyPostfix]
    private static void InitPostfix(ref TreasureBox __instance)
    {
        if (Singletons.RuntimeVariables is { } runtimeVariables && runtimeVariables.ChestOverrides.TryGetValue(__instance.name, out var itemOverride))
        {
            __instance.ItemType = itemOverride;
        }
        else
        {
            Plugin.Log.LogWarning($"Found unlisted chest shuffle: {__instance.name}");
        }
    }
}
