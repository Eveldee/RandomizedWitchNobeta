using HarmonyLib;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Runtime.Gameplay;

public static class CombatPatches
{
    // Damage received patches
    [HarmonyPatch(typeof(WizardGirlManage), nameof(WizardGirlManage.Hit))]
    [HarmonyPrefix]
    private static void WizardGirlManageHitPrefix(AttackData Data, bool bIgnoreDodge = false)
    {
        if (Singletons.RuntimeVariables is not { } runtimeVariables)
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
}