using System.Collections.Generic;
using HarmonyLib;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Features.Combat;

public static class DamageModifiersPatches
{
    // Used to avoid already modified attack data because the game doesn't create
    // new instances for each attack of the same source (e.g. fire traps)
    private static readonly HashSet<AttackData> _modifiedData = new(new Il2CppObjectComparer());

    // Damage received patches
    [HarmonyPatch(typeof(WizardGirlManage), nameof(WizardGirlManage.Hit))]
    [HarmonyPrefix]
    private static void WizardGirlManageHitPrefix(AttackData Data, bool bIgnoreDodge = false)
    {
        if (Singletons.RuntimeVariables is not { } runtimeVariables)
        {
            return;
        }

        if (!_modifiedData.Add(Data))
        {
            return;
        }

        // Ignore fire traps
        if (Data.name is "FireTrap")
        {
            return;
        }

        var settings = runtimeVariables.Settings;

        // One hit KO
        if (settings.OneHitKO)
        {
            Data.g_fStrength = float.MaxValue;
        }
        // Double damage
        else if (settings.DoubleDamage)
        {
            Data.g_fStrength *= 2;
        }
        // Half damage
        else if (settings.HalfDamage)
        {
            Data.g_fStrength /= 2;
        }
    }

    // Clear the modified data set to avoid it getting too big
    [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.OnSceneInitComplete))]
    [HarmonyPostfix]
    private static void OnSceneInitCompletePostfix()
    {
        _modifiedData.Clear();
    }
}