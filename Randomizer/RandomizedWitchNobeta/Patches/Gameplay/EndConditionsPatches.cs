using System.Linq;
using HarmonyLib;
using RandomizedWitchNobeta.Utils;
using UnityEngine;

namespace RandomizedWitchNobeta.Patches.Gameplay;

public static class EndConditionsPatches
{
    public const string CentralTeleportName = "PE_Teleport_RoomCentral04";

    private static Teleport _teleport;

    [HarmonyPatch(typeof(PlayerInputController), nameof(PlayerInputController.Interact))]
    [HarmonyPrefix]
    private static bool InputInteractPrefix(PlayerInputController __instance)
    {
        // Only check in last stage
        if (Game.sceneManager is not { stageId: 7 } || Singletons.RuntimeVariables is not { } runtimeVariables)
        {
            return true;
        }

        // If no special mode is enabled, just skip
        if (runtimeVariables.Settings is { BossHunt: false, MagicMaster: false })
        {
            return true;
        }

        bool allowInteraction = true;

        // Check if Nobeta is in range of the interact button
        var distance = Vector3.Distance(_teleport.transform.position, __instance.controller.wgm.transform.position);

        if (distance <= 2f)
        {
            var prompts = Game.stageUI.interactionPrompt.prompts;

            if (prompts.Any(prompt => prompt.gameObject.activeInHierarchy && prompt.content.text is "Teleport"))
            {
                // Magic master check
                if (runtimeVariables.Settings.MagicMaster)
                {
                    var stats = Game.GameSave.stats;
                    if (stats is not
                    {
                        secretMagicLevel: >= 5,
                        iceMagicLevel: >= 5,
                        fireMagicLevel: >= 5,
                        thunderMagicLevel: >= 5
                    })
                    {
                        Game.AppearEventPrompt("Only the Magic Master may pass.");

                        allowInteraction = false;
                    }
                }

                // Boss Hunt check
                if (runtimeVariables.Settings.BossHunt)
                {
                    if (runtimeVariables.KilledBosses.Count < NpcUtils.ValidBosses.Count)
                    {
                        Game.AppearEventPrompt("Only an accomplished Boss Hunter may pass.");

                        allowInteraction = false;
                    }
                }
            }
        }

        return allowInteraction;
    }

    [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.OnSceneInitComplete))]
    [HarmonyPostfix]
    private static void OnSceneInitCompletePostfix()
    {
        // Only check in last stage
        if (Game.sceneManager is not { stageId: 7 } || Singletons.RuntimeVariables is null)
        {
            return;
        }

        _teleport = UnityUtils.FindComponentByNameForced<Teleport>(CentralTeleportName);
    }
}