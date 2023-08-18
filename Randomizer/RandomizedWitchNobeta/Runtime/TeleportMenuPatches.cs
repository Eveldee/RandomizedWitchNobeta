using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace RandomizedWitchNobeta.Runtime;

public static class TeleportMenuPatches
{
    private static GameObject[] _topHandlers;

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
    }
}