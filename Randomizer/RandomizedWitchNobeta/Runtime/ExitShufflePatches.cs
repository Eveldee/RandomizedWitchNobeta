using HarmonyLib;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Runtime;

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

        (int sourceScene, int nextScene, int nextSavePoint) source = (Game.sceneManager.stageId, SceneUtils.SceneNumberFromName(sceneData.nextSceneName), sceneData.savePointNumber);

        // Do not patch unknown scenes (title screen, last cutscene, ...)
        if (source.nextScene == -1 || Singletons.RuntimeVariables is not { } runtimeVariables)
        {
            return true;
        }

        if (runtimeVariables.ExitsOverrides.TryGetValue(source, out var destination))
        {
            Game.SwitchScene(new SceneSwitchData(destination.sceneNumberOverride, destination.savePointOverride, true), fadeInDuration);

            return false;
        }

        Plugin.Log.LogInfo($"Skipping unlisted scene transition: {source}");

        return true;
    }
}