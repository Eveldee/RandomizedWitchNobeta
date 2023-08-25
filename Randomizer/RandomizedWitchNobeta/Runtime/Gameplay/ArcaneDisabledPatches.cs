using HarmonyLib;

namespace RandomizedWitchNobeta.Runtime.Gameplay;

public static class ArcaneDisabledPatches
{
    [HarmonyPatch(typeof(PlayerInputController), nameof(PlayerInputController.Shoot))]
    [HarmonyPrefix]
    public static bool InputShootPrefix(PlayerInputController __instance, bool onHolding)
    {
        var wizardGirl = __instance.controller.wgm;

        if (wizardGirl.GetMagicType() == PlayerEffectPlay.Magic.Null && wizardGirl.GameSave.stats.secretMagicLevel < 1)
        {
            if (onHolding)
            {
                Game.AppearEventPrompt("You need at least Arcane level 1 to perform this action.");
            }

            return false;
        }

        return true;
    }

    [HarmonyPatch(typeof(PlayerInputController), nameof(PlayerInputController.Chant))]
    [HarmonyPrefix]
    public static bool InputChantPrefix(PlayerInputController __instance)
    {
        var wizardGirl = __instance.controller.wgm;

        if (wizardGirl.GetMagicType() == PlayerEffectPlay.Magic.Null && wizardGirl.GameSave.stats.secretMagicLevel < 1)
        {
            Game.AppearEventPrompt("You need at least Arcane level 1 to perform this action.");

            return false;
        }

        return true;
    }
}