using HarmonyLib;
using RandomizedWitchNobeta.Generation.Models;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Runtime.Shuffle;

public static class ExitShufflePatches
{
    [HarmonyPatch(typeof(Game), nameof(Game.SwitchScene))]
    [HarmonyPrefix]
    private static bool SwitchScenePrefix(SceneSwitchData sceneData, float fadeInDuration)
    {
        if (Game.sceneManager is null)
        {
            return true;
        }

        var source = new RegionExit(Game.sceneManager.stageId, SceneUtils.SceneNumberFromName(sceneData.nextSceneName), sceneData.savePointNumber);

        // Do not patch unknown scenes (title screen, last cutscene, ...)
        if (source.NextSceneNumber == -1 || Singletons.RuntimeVariables is not { } runtimeVariables)
        {
            return true;
        }

        if (runtimeVariables.ExitsOverrides.TryGetValue(source, out var destination))
        {
            // Do nothing if the overriden destination is the exact same as the original destination (avoid infinite loop)
            if (source.NextSceneNumber == destination.sceneNumberOverride && source.NextSavePointNumber == destination.savePointOverride)
            {
                return true;
            }

            Game.SwitchScene(new SceneSwitchData(destination.sceneNumberOverride, destination.savePointOverride, true), fadeInDuration);

            return false;
        }

        Plugin.Log.LogInfo($"Skipping unlisted scene transition: {source}");

        return true;
    }

    // Fix text display on doors
    [HarmonyPatch(typeof(StageUIManager), nameof(StageUIManager.AppearExitLevelMsgBox))]
    [HarmonyPostfix]
    private static void AppearExitLevelPostfix(StageUIManager __instance, SavePoint savePointData)
    {
        // Display override destination if it exists
        if (Singletons.RuntimeVariables is not { } runtimeVariables)
        {
            return;
        }

        Plugin.Log.LogDebug($"{Singletons.GameUIManager.messageBoxMap[MessageBoxStyle.Universal].title.text}");

        if (runtimeVariables.ExitsOverrides.TryGetValue(
                new RegionExit(Game.sceneManager.stageId, savePointData.TransferLevelNumber, savePointData.TransferSavePointNumber),
                out var destination))
        {
            Singletons.GameUIManager.messageBoxMap[MessageBoxStyle.Universal].title.text =
                $"Leave for {SceneUtils.FriendlySceneName(destination.sceneNumberOverride)}?";
        }
    }
}