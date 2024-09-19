using HarmonyLib;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Features.ItemChecks;

public static class ChestExtraLootPatches
{
    [HarmonyPatch(typeof(TreasureBox), nameof(TreasureBox.SetOpen))]
    [HarmonyPostfix]
    private static void OpenPostfix(TreasureBox __instance)
    {
        if (__instance.ItemType == ItemSystem.ItemType.Null)
        {
            Game.CreateSoul(SoulSystem.SoulType.Money, __instance.transform.position, Singletons.RuntimeVariables.Settings.ChestSoulCount);
        }
    }
}