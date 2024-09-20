using System;
using System.IO;
using HarmonyLib;
using RandomizedWitchNobeta.Utils;
using RandomizedWitchNobeta.Utils.Nobeta;

namespace RandomizedWitchNobeta.Features.UI;

public static class RunCompletePatches
{
    public static readonly DirectoryInfo NobetaSavesDirectory = new(
        Path.Combine(Plugin.ConfigDirectory.FullName, "..", "..", "..", "LittleWitchNobeta_Data", "Save")
    );

    [HarmonyPatch(typeof(NPCManage), nameof(NPCManage.Hit))]
    [HarmonyPostfix]
    private static void NpcHitPostfix(NPCManage __instance)
    {
        if (Singletons.RuntimeVariables is not { } runtimeVariables)
        {
            return;
        }

        // Stop timer directly when killing Nonota since we want to let players watch the
        // credits without being impacted
        if (__instance.name is NpcUtils.NonotaBossName && __instance.GetIsDeath())
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

            Seed Hash: {runtimeVariables.Settings.Hash(StartPatches.GameVersionText, StartPatches.RandomizerVersionText):X8}
            """;

            beatingGameMessageBox.config.titleText = text;
            beatingGameMessageBox.title.text = text;

            var seedHash = Singletons.RuntimeVariables.Settings.Hash(StartPatches.GameVersionText, StartPatches.RandomizerVersionText);
            Singletons.RuntimeVariables = null;

            ArchiveRun(seedHash, StartPatches.GameVersionText, StartPatches.RandomizerVersionText);
        }
    }

    private static void ArchiveRun(int seedHash, string gameVersion, string randomizerVersion)
    {
        string runName = $"{DateTime.Now.ToString("s").Replace(':', '.')} - {seedHash:X8} - {gameVersion[^5..]} - {randomizerVersion[^5..]}";

        var destinationDirectory = Path.Combine(Plugin.ConfigDirectory.FullName, "PastRuns", runName);
        Directory.CreateDirectory(destinationDirectory);

        File.Move(RuntimeVariables.SavePath, Path.Combine(destinationDirectory, "RuntimeVariables.json"));
        File.Move(
            Path.Combine(NobetaSavesDirectory.FullName, $"GameSave{StartPatches.GameSaveIndex:D2}.dat"),
            Path.Combine(destinationDirectory, "GameSave.dat"
        ));
    }
}