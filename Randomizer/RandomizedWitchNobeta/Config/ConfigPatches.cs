using System;
using HarmonyLib;

namespace RandomizedWitchNobeta.Config;

public static class ConfigPatches
{
    [HarmonyPatch(typeof(Game), nameof(Game.WriteGameSave), [])]
    [HarmonyPostfix]
    public static void WriteGameSavePostfix()
    {
        Plugin.Log.LogDebug("Triggered Config save on Game save");

        Plugin.SaveConfigs();
    }
}