using HarmonyLib;
using RandomizedWitchNobeta.Generation.Models;
using RandomizedWitchNobeta.Utils;
using RandomizedWitchNobeta.Utils.Nobeta;

namespace RandomizedWitchNobeta.Features.Locations;

public static class ExitShufflePatches
{
    private static bool _switchInProgress = false;

    [HarmonyPatch(typeof(Game), nameof(Game.SwitchScene))]
    [HarmonyPrefix]
    private static bool SwitchScenePrefix(SceneSwitchData sceneData, float fadeInDuration)
    {
        if (_switchInProgress)
        {
            return true;
        }

        _switchInProgress = true;

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

            // Also update save point to avoid wrong "return to statue"
            Game.GameSave.basic.stage = SceneUtils.SceneNumberToGameStage(destination.sceneNumberOverride);
            Game.GameSave.basic.savePoint = destination.savePointOverride;

            return false;
        }

        Plugin.Log.LogInfo($"Skipping unlisted scene transition: {source}");

        return true;
    }

    [HarmonyPatch(typeof(Game), nameof(Game.SwitchScene))]
    [HarmonyPostfix]
    private static void SwitchScenePost(SceneSwitchData sceneData, float fadeInDuration)
    {
        _switchInProgress = false;
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

        if (runtimeVariables.ExitsOverrides.TryGetValue(
                new RegionExit(Game.sceneManager.stageId, savePointData.TransferLevelNumber, savePointData.TransferSavePointNumber),
                out var destination))
        {
            Singletons.GameUIManager.messageBoxMap[MessageBoxStyle.Universal].title.text =
                $"Leave for {SceneUtils.FriendlySceneName(destination.sceneNumberOverride)}?";
        }
    }
}