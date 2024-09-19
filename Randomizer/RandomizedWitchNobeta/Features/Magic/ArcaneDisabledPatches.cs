using HarmonyLib;

namespace RandomizedWitchNobeta.Features.Magic;

public static class ArcaneDisabledPatches
{
    [HarmonyPatch(typeof(PlayerInputController), nameof(PlayerInputController.Shoot))]
    [HarmonyPrefix]
    private static bool InputShootPrefix(PlayerInputController __instance, bool onHolding)
    {
        var wizardGirl = __instance.controller.wgm;

        if (wizardGirl.GetMagicType() == PlayerEffectPlay.Magic.Null && wizardGirl.GameSave.stats.secretMagicLevel < 1)
        {
            if (onHolding)
            {
                Game.AppearEventPrompt("You have yet to learn Arcane magic.");
            }

            return false;
        }

        return true;
    }

    [HarmonyPatch(typeof(PlayerInputController), nameof(PlayerInputController.Chant))]
    [HarmonyPrefix]
    private static bool InputChantPrefix(PlayerInputController __instance)
    {
        var wizardGirl = __instance.controller.wgm;

        if (wizardGirl.GetMagicType() == PlayerEffectPlay.Magic.Null && wizardGirl.GameSave.stats.secretMagicLevel < 1)
        {
            Game.AppearEventPrompt("You have yet to learn Arcane magic.");

            return false;
        }

        return true;
    }
}