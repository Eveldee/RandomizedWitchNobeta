using HarmonyLib;
using RandomizedWitchNobeta.Generation;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Patches.Gameplay;

public static class MagicUpgradePatches
{
    // Update magic level on magic unlocked
    [HarmonyPatch(typeof(WizardGirlManage), nameof(WizardGirlManage.GetMagicLevelSuffix))]
    [HarmonyPostfix]
    private static void GetMagicLevelSuffixPostfix(WizardGirlManage __instance)
    {
        if (Singletons.RuntimeVariables is { } runtimeVariables)
        {
            // Skip if magic upgrade mode is not Boss Kill
            if (runtimeVariables.Settings.MagicUpgrade != SeedSettings.MagicUpgradeMode.BossKill)
            {
                return;
            }

            // Update all unlocked magic levels according to global level
            UpdateMagicLevels(__instance.GameSave.stats);
        }
    }

    [HarmonyPatch(typeof(NPCManage), nameof(NPCManage.Hit))]
    [HarmonyPostfix]
    private static void NpcHitPostfix(NPCManage __instance)
    {
        if (Singletons.RuntimeVariables is not { } runtimeVariables)
        {
            return;
        }

        // Do not skip directly since we need it for end conditions too
        // if (runtimeVariables.Settings.MagicUpgrade != SeedSettings.MagicUpgradeMode.BossKill)
        // {
        //     return;
        // }

        // Track boss death
        if (NpcUtils.ValidBosses.Contains(__instance.name) && __instance.GetIsDeath())
        {
            if (runtimeVariables.KilledBosses.Add(__instance.name))
            {
                // A boss has been killed, increase global magic level since it's the first time it got killed
                if (runtimeVariables.Settings.MagicUpgrade == SeedSettings.MagicUpgradeMode.BossKill && runtimeVariables.GlobalMagicLevel < 5)
                {
                    runtimeVariables.GlobalMagicLevel++;

                    UpdateMagicLevels(Game.GameSave.stats);

                    Game.AppearEventPrompt($"All magic levels increased to {runtimeVariables.GlobalMagicLevel}.");
                    Plugin.Log.LogDebug($"Global magic level increased after killing '{__instance.name}'");
                }
            }
        }
    }

    private static void UpdateMagicLevels(PlayerStatsData stats)
    {
        var globalLevel = Singletons.RuntimeVariables.GlobalMagicLevel;

        if (stats.secretMagicLevel > 0)
        {
            stats.secretMagicLevel = globalLevel;
        }

        if (stats.iceMagicLevel > 0)
        {
            stats.iceMagicLevel = globalLevel;
        }

        if (stats.fireMagicLevel > 0)
        {
            stats.fireMagicLevel = globalLevel;
        }

        if (stats.thunderMagicLevel > 0)
        {
            stats.thunderMagicLevel = globalLevel;
        }

        stats.windMagicLevel = globalLevel;
        stats.manaAbsorbLevel = globalLevel;
    }
}