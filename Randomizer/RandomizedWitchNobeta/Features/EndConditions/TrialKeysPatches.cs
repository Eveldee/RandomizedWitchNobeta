using System.Collections.Generic;
using HarmonyLib;
using RandomizedWitchNobeta.Utils;
using RandomizedWitchNobeta.Utils.Extensions;
using UnityEngine;

namespace RandomizedWitchNobeta.Features.EndConditions;

public static class TrialKeysPatches
{
    private static readonly List<MultipleEventOpen> _openers = [];

    private static string _lastOpenerEnteredName = "";

    // Disable auto-open of trials
    [HarmonyPatch(typeof(MultipleEventOpen), nameof(MultipleEventOpen.InitData))]
    [HarmonyPostfix]
    private static void MultipleEventOpenInitPostfix(MultipleEventOpen __instance)
    {
        // Only check in last stage
        if (Game.sceneManager.stageId != 7 || Singletons.RuntimeVariables is not { } runtimeVariables)
        {
            return;
        }

        // Skip if trial keys are not enabled
        if (!runtimeVariables.Settings.TrialKeys)
        {
            return;
        }

        if (__instance.name is "OpenLightRoomStart01" or "OpenLightRoomStart02" or "OpenLightRoomStart03")
        {
            // Reopen it if it has already been opened
            if (runtimeVariables.OpenedTrials.Contains(__instance.name))
            {
                __instance.OpenEvent();

                return;
            }

            __instance.CheckPlayerEnter = false;

            // Make collider smaller so they don't overlap
            var extents = __instance.g_BC.extents - new Vector3(3f, 0f, 3f);
            __instance.g_BC.extents = extents;

            _openers.Add(__instance);
        }
    }

    [HarmonyPatch(typeof(Game), nameof(Game.EnterLoaderScene))]
    [HarmonyPrefix]
    private static void EnterLoaderScenePostfix()
    {
        _openers.Clear();
    }

    // Open trial on token drop
    [HarmonyPatch(typeof(PlayerItem), nameof(PlayerItem.DiscardItemSuccess))]
    [HarmonyPostfix]
    private static void DiscardItemPostfix(IItemController __instance)
    {
        if (Game.sceneManager.stageId != 7 || Singletons.RuntimeVariables is not { } runtimeVariables)
        {
            return;
        }

        // Skip if trial keys are not enabled
        if (!runtimeVariables.Settings.TrialKeys)
        {
            return;
        }

        var items = UnityUtils.FindComponentsByTypeForced<Item>();

        // Check if any token is in a trial open bound
        foreach (var item in items)
        {
            if (item.currentItemType == ItemSystem.ItemType.SPMaxAdd)
            {
                foreach (var eventOpen in _openers)
                {
                    if (!eventOpen.g_AllOpen && eventOpen.g_BC.Contains(item.transform.position))
                    {
                        eventOpen.OpenEvent();
                        runtimeVariables.OpenedTrials.Add(eventOpen.name);

                        Object.Destroy(item.gameObject);

                        return;
                    }
                }
            }
        }
    }

    // Display a help message when near a trial
    [HarmonyPatch(typeof(WizardGirlManage), nameof(WizardGirlManage.Update))]
    [HarmonyPostfix]
    private static void HelpMessageUpdatePostfix(WizardGirlManage __instance)
    {
        // Skip if trial keys are not enabled
        if (Game.sceneManager.stageId != 7 || Singletons.RuntimeVariables is not { Settings.TrialKeys: true } runtimeVariables)
        {
            return;
        }

        foreach (var opener in _openers)
        {
            if (opener.name != _lastOpenerEnteredName
                && opener.g_BC.Contains(__instance.transform.position)
                && !runtimeVariables.OpenedTrials.Contains(opener.name))
            {
                _lastOpenerEnteredName = opener.name;

                Game.AppearEventPrompt("Drop a trial key to open the trial teleporter.");

                return;
            }
        }
    }

    // Disable usage of tokens (can only drop)
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.OnUseItemHotKeyDown))]
    [HarmonyPrefix]
    private static bool UseItemPrefix(PlayerController __instance, int index)
    {
        return CheckUseItem(__instance.g_Item.GetSelectItemType(index));
    }

    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.OnUseItemKeyDown))]
    [HarmonyPrefix]
    private static bool UseItemPrefix(PlayerController __instance)
    {
        return CheckUseItem(__instance.g_Item.GetSelectItemType(Game.GetItemSelectPos()));
    }

    private static bool CheckUseItem(ItemSystem.ItemType itemType)
    {
        if (itemType == ItemSystem.ItemType.SPMaxAdd)
        {
            Game.AppearEventPrompt("Trial keys can only be dropped, not used.");

            return false;
        }

        return true;
    }

    // Patch display of token item name and description
    [HarmonyPatch(typeof(ItemSystem), nameof(ItemSystem.GetItemHelp))]
    [HarmonyPrefix]
    private static bool GetItemHelpPrefix(ref string __result, ItemSystem.ItemType Type)
    {
        if (Type == ItemSystem.ItemType.SPMaxAdd)
        {
            __result = "Cannot be used.\nDrop on a trial path to unlock it.";

            return false;
        }

        return true;
    }

    [HarmonyPatch(typeof(ItemSystem), nameof(ItemSystem.GetItemName))]
    [HarmonyPrefix]
    private static bool GetItemNamePrefix(ref string __result, ItemSystem.ItemType Type)
    {
        if (Type == ItemSystem.ItemType.SPMaxAdd)
        {
            __result = "Trial Key";

            return false;
        }

        return true;
    }
}