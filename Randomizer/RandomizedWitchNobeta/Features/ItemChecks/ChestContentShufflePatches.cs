﻿using HarmonyLib;
using RandomizedWitchNobeta.Generation.Models;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Features.ItemChecks;

public static class ChestContentShufflePatches
{
    [HarmonyPatch(typeof(TreasureBox), nameof(TreasureBox.Init))]
    [HarmonyPostfix]
    private static void InitPostfix(ref TreasureBox __instance)
    {
        if (Singletons.RuntimeVariables is { } runtimeVariables && runtimeVariables.ChestOverrides.TryGetValue(new ChestOverride(__instance.name, Game.sceneManager.stageId), out var itemOverride))
        {
            __instance.ItemType = itemOverride;
        }
        else
        {
            Plugin.Log.LogWarning($"Found unlisted chest shuffle: {__instance.name}");
        }
    }
}
