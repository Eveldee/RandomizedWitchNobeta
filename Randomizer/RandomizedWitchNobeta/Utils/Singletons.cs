using HarmonyLib;
using RandomizedWitchNobeta.Behaviours;
using RandomizedWitchNobeta.Features;
using RandomizedWitchNobeta.Features.Timer;
using UnityEngine;

namespace RandomizedWitchNobeta.Utils;

public static class Singletons
{
    public static Game GameInstance { get; private set; }
    public static NobetaSkin NobetaSkin { get; private set; }
    public static WizardGirlManage WizardGirl { get; private set; }
    public static PlayerController PlayerController => WizardGirl?.playerController;
    public static PlayerInputController InputController => WizardGirl?.inputController;
    public static CharacterController CharacterController => WizardGirl?.characterController;
    public static NobetaRuntimeData RuntimeData => WizardGirl?.playerController?.runtimeData;
    public static SceneManager SceneManager => Game.sceneManager;
    public static ItemSystem ItemSystem => SceneManager?.itemSystem;
    public static GameSave GameSave { get; set; }
    public static GameSettings GameSettings => Game.Config?.gameSettings;
    public static StageUIManager StageUi => Game.stageUI;
    public static UnityMainThreadDispatcher Dispatcher => UnityMainThreadDispatcher.Instance;
    public static Timers Timers { get; set; }
    public static UIPauseMenu UIPauseMenu { get; private set; }
    public static GameUIManager GameUIManager { get; private set; }

    public static SettingsService SettingsService { get; set; }

    public static bool SaveLoaded => GameSave is not null;

    public static RuntimeVariables RuntimeVariables { get; set; }

    [HarmonyPatch(typeof(Game), nameof(Game.Awake))]
    [HarmonyPostfix]
    private static void GameAwakePostfix(Game __instance)
    {
        GameInstance = __instance;
    }

    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.UpdateSkin))]
    [HarmonyPostfix]
    private static void PlayerControllerUpdateSkinPostfix(PlayerController __instance, NobetaSkin skin)
    {
        Plugin.Log.LogInfo("NobetaSkin updated");

        NobetaSkin = skin;
    }

    [HarmonyPatch(typeof(NobetaSkin), nameof(NobetaSkin.Dispose))]
    [HarmonyPrefix]
    private static void NobetaSkinDisposePrefix(NobetaSkin __instance)
    {
        Plugin.Log.LogInfo("NobetaSkin disposed");

        if (__instance.Pointer == NobetaSkin?.Pointer)
        {
            NobetaSkin = null;
        }
    }

    [HarmonyPatch(typeof(WizardGirlManage), nameof(WizardGirlManage.Init))]
    [HarmonyPostfix]
    private static void WizardGirlManageInitPostfix(WizardGirlManage __instance)
    {
        Plugin.Log.LogInfo("WizardGirlManage created");

        WizardGirl = __instance;
    }

    [HarmonyPatch(typeof(WizardGirlManage), nameof(WizardGirlManage.Dispose))]
    [HarmonyPrefix]
    private static void WizardGirlManageDisposePrefix(WizardGirlManage __instance)
    {
        Plugin.Log.LogInfo("WizardGirlManage disposed");

        WizardGirl = null;
    }

    [HarmonyPatch(typeof(Game), nameof(Game.SwitchGameSave))]
    [HarmonyPostfix]
    private static void StartGamePlayPostfix(GameSave gameSave)
    {
        Plugin.Log.LogInfo("Save loaded");

        GameSave = gameSave;
    }

    [HarmonyPatch(typeof(Game), nameof(Game.SwitchTitleScene))]
    [HarmonyPostfix]
    private static void SwitchTitleScenePostfix()
    {
        GameSave = null;
    }

    [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.Enter))]
    [HarmonyPostfix]
    private static void EnterScenePostfix()
    {
        Plugin.Log.LogInfo("Entered scene");
    }

    [HarmonyPatch(typeof(UIPauseMenu), nameof(UIPauseMenu.Init))]
    [HarmonyPostfix]
    private static void UIPauseMenuInit(UIPauseMenu __instance)
    {
        Plugin.Log.LogInfo("UIPauseMenu Init");

        UIPauseMenu = __instance;
    }

    [HarmonyPatch(typeof(GameUIManager), nameof(global::GameUIManager.Init))]
    [HarmonyPostfix]
    private static void GameUIManagerInit(GameUIManager __instance)
    {
        Plugin.Log.LogInfo("GameUIManager Init");

        GameUIManager = __instance;
    }
}