using HarmonyLib;
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

        (int sourceScene, int nextScene, int nextSavePoint) source = (Game.sceneManager.stageId, SceneUtils.SceneNumberFromName(sceneData.nextSceneName), sceneData.savePointNumber);

        // Do not patch unknown scenes (title screen, last cutscene, ...)
        if (source.nextScene == -1 || Singletons.RuntimeVariables is not { } runtimeVariables)
        {
            return true;
        }

        if (runtimeVariables.ExitsOverrides.TryGetValue(source, out var destination))
        {
            // Do nothing if the overriden destination is the exact same as the original destination (avoid infinite loop)
            if (source.nextScene == destination.sceneNumberOverride && source.nextSavePoint == destination.savePointOverride)
            {
                return true;
            }

            Game.SwitchScene(new SceneSwitchData(destination.sceneNumberOverride, destination.savePointOverride, true), fadeInDuration);

            return false;
        }

        Plugin.Log.LogInfo($"Skipping unlisted scene transition: {source}");

        return true;
    }
}