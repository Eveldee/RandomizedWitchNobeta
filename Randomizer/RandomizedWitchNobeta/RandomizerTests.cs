using HarmonyLib;

namespace RandomizedWitchNobeta;

public static class RandomizerTests
{
    // [HarmonyPatch(typeof(TreasureBox), nameof(TreasureBox.Init))]
    // [HarmonyPostfix]
    // private static void InitPostfix(ref TreasureBox __instance)
    // {
    //     __instance.ItemType = ItemSystem.ItemType.MagicLightning;
    // }

    [HarmonyPatch(typeof(TreasureBox), nameof(TreasureBox.SetOpen))]
    [HarmonyPrefix]
    private static void SetOpenPrefix(ref TreasureBox __instance)
    {
        Plugin.Log.LogDebug($"Opened chest '{__instance.name}\t| {__instance.ItemType}'");
    }

    [HarmonyPatch(typeof(Game), nameof(Game.SwitchScene))]
    [HarmonyPrefix]
    private static bool SwitchScenePrefix(SceneSwitchData sceneData, float fadeInDuration)
    {
        Plugin.Log.LogInfo($"Switching scene to '{sceneData.nextSceneName}' - '{sceneData.savePointNumber}'");

        // if (sceneData.nextSceneName == GameStage.Act03_01.ToString())
        // {
        //     Game.SwitchScene(new SceneSwitchData(GameStage.Act06_03, -1, false), fadeInDuration);
        //
        //     return false;
        // }

        return true;
    }
}