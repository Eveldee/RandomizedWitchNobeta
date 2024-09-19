using System;
using System.Linq;
using HarmonyLib;

namespace RandomizedWitchNobeta.Utils.Nobeta;

public static class SceneUtils
{
    public static bool IsGameScene;

    public static bool IsLoading => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Loader";

    public static AreaCheck FindLastAreaCheck()
    {
        var sceneHides = UnityUtils.FindComponentsByTypeForced<SceneIsHide>().Where(hide => !hide.g_bIsHide);
        var areaChecks = UnityUtils.FindComponentsByTypeForced<AreaCheck>();

        var checks = areaChecks.Where(areaCheck => sceneHides.All(hide =>
            areaCheck.ShowArea.Any(area => hide.gameObject.GetInstanceID() == area.GetInstanceID()))
        );

        return checks.First(check => check.ShowArea.Count == sceneHides.Count());
    }

    public static void ReturnToStatue()
    {
        Singletons.Dispatcher.Enqueue(() =>
        {
            if (IsGameScene && Singletons.UIPauseMenu is { } pauseMenu)
            {
                pauseMenu.ReloadStage();
            }
        });
    }

    public static void ReturnToTitleScreen()
    {
        Singletons.Dispatcher.Enqueue(() =>
        {
            UiHelpers.ForceCloseAllUi();
            Game.SwitchTitleScene(false);
        });
    }

    public static int SceneStartSavePoint(int destinationScene) => destinationScene switch
    {
        2 => -1,
        3 => -1,
        4 => 2,
        5 => 3,
        6 => -1,
        7 => -1,
        _ => throw new ArgumentOutOfRangeException(nameof(destinationScene), destinationScene, null)
    };

    [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.OnSceneInitComplete))]
    [HarmonyPostfix]
    private static void OnSceneInitCompletePostfix()
    {
        Plugin.Log.LogDebug($"New scene init complete: {Game.sceneManager.stageName}");

        IsGameScene = true;
    }

    [HarmonyPatch(typeof(Game), nameof(Game.EnterLoaderScene))]
    [HarmonyPrefix]
    private static void EnterLoaderScenePrefix()
    {
        Plugin.Log.LogDebug("Entered loader scene");

        IsGameScene = false;
    }

    public static int SceneNumberFromName(string sceneName) => sceneName switch
    {
        "Act02_01" => 2,
        "Act03_01" => 3,
        "Act04_01" => 4,
        "Act05_02" => 5,
        "Act06_03" => 6,
        "Act07" => 7,
        _ => -1
    };

    public static GameStage SceneNumberToGameStage(int sceneNumber) => sceneNumber switch
    {
        2 => GameStage.Act02_01,
        3 => GameStage.Act03_01,
        4 => GameStage.Act04_01,
        5 => GameStage.Act05_02,
        6 => GameStage.Act06_03,
        7 => GameStage.Act07,
        _ => throw new ArgumentOutOfRangeException(nameof(sceneNumber), sceneNumber, null)
    };

    public static string FriendlySceneName(int sceneNumber) => sceneNumber switch
    {
        2 => "Okun Shrine - Entrance Hall",
        3 => "Okun Shrine - Underground Cave",
        4 => "Lava Ruins - Lobby",
        5 => "Dark Tunnel - Entrance",
        6 => "Spirit Realm - Garden",
        7 => "Abyss - Sky Walk",
        _ => throw new ArgumentOutOfRangeException(nameof(sceneNumber), sceneNumber, null)
    };

    public static int SceneStartSouls(int sceneNumber) => sceneNumber switch
    {
        2 => 0,
        3 => 60,
        4 => 500,
        5 => 900,
        6 => 1300,
        _ => throw new ArgumentOutOfRangeException(nameof(sceneNumber), sceneNumber, null)
    };
}