using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace RandomizedWitchNobeta.Features.UI;

public static class TeleportMenuPatches
{
    private static GameObject[] _topHandlers;
    private static UILabelHandler _returnHandler;
    private static UILabelHandler _exitHandler;
    private static UITeleportHandler _firstStage6Handler;
    private static UITeleportHandler _firstStage1Handler;

    [HarmonyPatch(typeof(UITeleport), nameof(UITeleport.Init))]
    [HarmonyPostfix]
    private static void UITeleportInitPostfix(UITeleport __instance)
    {
        var root = __instance.transform;

        var handlers = root.Find("TopHandlers");
        var grid = handlers.GetComponent<GridLayoutGroup>();

        var background = root.Find("Background");

        // Scale background
        background.localScale = background.localScale with { x = 1.28f };

        // Enable grid and change layout
        grid.enabled = true;
        grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        grid.constraintCount = 4;
        grid.spacing = new Vector2(10f, 10f);

        // Store top handlers for later use
        _topHandlers = Enumerable.Range(0, handlers.childCount).Select(index => handlers.GetChild(index).gameObject).ToArray();

        // Change how handlers are navigated to allow better controller support
        var uiHandlers = handlers.GetComponentsInChildren<UITeleportHandler>(true);

        var stage1Handlers = uiHandlers.Where(uiHandler => uiHandler.name.StartsWith("Handler_1")).ToArray();
        var stage2Handlers = uiHandlers.Where(uiHandler => uiHandler.name.StartsWith("Handler_2")).ToArray();
        var stage3Handlers = uiHandlers.Where(uiHandler => uiHandler.name.StartsWith("Handler_3")).ToArray();
        var stage4Handlers = uiHandlers.Where(uiHandler => uiHandler.name.StartsWith("Handler_4")).ToArray();
        var stage5Handlers = uiHandlers.Where(uiHandler => uiHandler.name.StartsWith("Handler_5")).ToArray();
        var stage6Handlers = uiHandlers.Where(uiHandler => uiHandler.name.StartsWith("Handler_6")).ToArray();

        // Vertical navigation
        for (int i = 0; i < stage4Handlers.Length; i++)
        {
            stage4Handlers[i].selectDown = stage5Handlers[i];
            stage5Handlers[i].selectUp = stage4Handlers[i];
        }

        // Horizontal navigation
        stage1Handlers[0].selectLeft = stage5Handlers[^1];
        stage2Handlers[0].selectLeft = stage6Handlers[^1];
        stage1Handlers[^1].selectRight = stage5Handlers[0];
        stage2Handlers[^1].selectRight = stage6Handlers[0];

        stage5Handlers[0].selectLeft = stage1Handlers[^1];
        stage6Handlers[0].selectLeft = stage2Handlers[^1];
        stage5Handlers[^1].selectRight = stage1Handlers[0];
        stage6Handlers[^1].selectRight = stage2Handlers[0];

        _returnHandler = root.Find("ButtonHandlers/Back").GetComponent<UILabelHandler>();
        _exitHandler = root.Find("ButtonHandlers/Close").GetComponent<UILabelHandler>();
        _firstStage1Handler = stage1Handlers[0];
        _firstStage6Handler = stage6Handlers[0];

        Plugin.Log.LogInfo("Patched ui teleport menu");
    }

    [HarmonyPatch(typeof(UITeleport), nameof(UITeleport.Appear))]
    [HarmonyPostfix]
    private static void UITeleportAppearPostfix()
    {
        foreach (var topHandler in _topHandlers)
        {
            topHandler.SetActive(true);
        }

        _returnHandler.selectDown = _firstStage1Handler;
        _returnHandler.selectUp = _firstStage6Handler;
        _exitHandler.selectDown = _firstStage1Handler;
        _exitHandler.selectUp = _firstStage6Handler;
    }
}