using System.IO;
using HarmonyLib;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Runtime;

public static class RunCompletePatches
{
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.OnBeatingGame))]
    [HarmonyPostfix]
    private static void OnBeatingGamePostfix()
    {
        Plugin.Log.LogMessage("Run completed!");

        Singletons.RuntimeVariables = null;
        File.Delete(RuntimeVariables.SavePath);
    }
}