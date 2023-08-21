using System;
using HarmonyLib;
using RandomizedWitchNobeta.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RandomizedWitchNobeta.Runtime;

public static class SpecialLootPatches
{
    [HarmonyPatch(typeof(CatEvent), nameof(CatEvent.OpenEvent))]
    [HarmonyPrefix]
    private static void CatEventPrefix(CatEvent __instance)
    {
        if (Singletons.RuntimeVariables is not { } runtimeVariables)
        {
            return;
        }

        // Generate a random item
        if (__instance.name is "04_CatAbsorbSkill" or "04_SkillBookAgain")
        {
            var spawnPosition = __instance.name switch
            {
                "04_CatAbsorbSkill" => new Vector3(53.40f, 0.35f, -6.47f),
                "04_SkillBookAgain" =>  new Vector3(63.30f, 21.69f, -44.72f),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (runtimeVariables.CatOverride != ItemSystem.ItemType.Null)
            {
                Singletons.ItemSystem.itemPoolMap[runtimeVariables.CatOverride]
                    .NewUse(spawnPosition, Quaternion.identity, false);
            }
            // Give souls
            else
            {
                Game.CreateSoul(SoulSystem.SoulType.Money, __instance.transform.position, Singletons.RuntimeVariables.Settings.ChestSoulCount);
            }
        }
    }

    [HarmonyPatch(typeof(CatEvent), nameof(CatEvent.OpenEvent))]
    [HarmonyPostfix]
    private static void CatEventPostfix(CatEvent __instance)
    {
        // Delete game generated absorb book (53.4, 0.4, -6.5) || (63.3, 21.7, -44.7)
        if (__instance.name is "04_CatAbsorbSkill" or "04_SkillBookAgain")
        {
            var spawnPosition = __instance.name switch
            {
                "04_CatAbsorbSkill" => new Vector3(53.4071083f, 0.350891441f, -6.4791069f),
                "04_SkillBookAgain" =>  new Vector3(63.3090019f, 21.6949997f, -44.7179985f),
                _ => throw new ArgumentOutOfRangeException()
            };

            var items = UnityUtils.FindComponentsByTypeForced<Item>();

            foreach (var item in items)
            {
                if (item.transform.position == spawnPosition)
                {
                    Object.Destroy(item.gameObject);
                }
            }
        }
    }
}