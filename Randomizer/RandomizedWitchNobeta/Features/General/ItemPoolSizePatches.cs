using HarmonyLib;

namespace RandomizedWitchNobeta.Features.General;

// Increase maximum amount of item of same type present
public static class ItemPoolSizePatches
{
    [HarmonyPatch(typeof(ItemSystem), nameof(ItemSystem.Init))]
    [HarmonyPostfix]
    private static void ItemSystemInitPostfix(ItemSystem __instance)
    {
        foreach (var itemPool in __instance.itemPoolMap.Values)
        {
            itemPool.pooledItems._limitation = 99;
        }
    }
}