using System;
using System.IO;
using HarmonyLib;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Patches.UI;

public static class RunCompletePatches
{
    [HarmonyPatch(typeof(NPCManage), nameof(NPCManage.Hit))]
    [HarmonyPostfix]
    private static void NpcHitPostfix(NPCManage __instance)
    {
        if (Singletons.RuntimeVariables is not { } runtimeVariables)
        {
            return;
        }

        // A boss has been killed, increase global magic level if it's the first time it got killed
        if (__instance.name is "Boss_Level06" && __instance.GetIsDeath())
        {
            Plugin.Log.LogDebug("Timer ended!");
            Singletons.Timers.End();
        }
    }

    [HarmonyPatch(typeof(StaffManager), nameof(StaffManager.ShowBeatingMessageBox))]
    [HarmonyPostfix]
    private static void ShowBeatingMessageBoxPostfix(StaffManager __instance)
    {
        Plugin.Log.LogMessage("Run completed!");
        if (Singletons.RuntimeVariables is { } runtimeVariables)
        {
            var beatingGameMessageBox = Singletons.GameUIManager.GetMessageBox(MessageBoxStyle.BeatingGame);
            var text =
            $"""
            Congratulations for completing this randomizer run!

                       Real Time: {runtimeVariables.ElapsedRealTime.ToString(FormatUtils.TimeSpanMillisFormat)}
            Load Removed: {runtimeVariables.ElapsedLoadRemoved.ToString(FormatUtils.TimeSpanMillisFormat)}

            Seed Hash: {runtimeVariables.Settings.Hash():X8}
            """;

            beatingGameMessageBox.config.titleText = text;
            beatingGameMessageBox.title.text = text;

            Singletons.RuntimeVariables = null;

            ArchiveRun();
        }
    }

    private static void ArchiveRun()
    {
        var filePath = RuntimeVariables.SavePath;
        var destinationDirectory = Path.Combine(Plugin.ConfigDirectory.FullName, "PastRuns");
        var destinationFilePath =
            Path.Combine(destinationDirectory, $"{DateTime.Now.ToString("s").Replace(':', '.')}.json");

        Directory.CreateDirectory(destinationDirectory);

        File.Move(filePath, destinationFilePath);
    }
}