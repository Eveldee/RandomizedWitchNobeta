using HarmonyLib;
using UnityEngine;

namespace RandomizedWitchNobeta.Features.General;

public static class LockedDoorsPatches
{
    [HarmonyPatch(typeof(SavePoint), nameof(SavePoint.Start))]
    [HarmonyPostfix]
    private static void SavePointInitPostfix(SavePoint __instance)
    {
        // Disabled specified doors
        if (__instance.name is "02_SavePointTransfer_RoomToLevel02" or "03_SavePointTransfer_ToLevel03")
        {
            // Disable collider to avoid being able to use the door
            var collider = __instance.gameObject.GetComponent<BoxCollider>();

            collider.enabled = false;
        }
    }
}