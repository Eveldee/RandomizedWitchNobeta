using System;
using HarmonyLib;
using RandomizedWitchNobeta.Shared;
using RandomizedWitchNobeta.Utils;
using Random = System.Random;

namespace RandomizedWitchNobeta.Features.Bonus;

public static class AppearancePatches
{
    public static GameSkin SelectedSkin;
    public static readonly string[] AvailableSkins = Enum.GetNames<GameSkin>();

    public static bool HideBagEnabled;
    public static bool HideStaffEnabled;
    public static bool HideHatEnabled;

    public static BonusSettings.RandomSkin RandomizeSkin;

    private static void UpdateSelectedSkin()
    {
        var gameSkin = SelectedSkin;

        Singletons.Dispatcher.Enqueue(() =>
        {
            Game.Collection.UpdateSkin(gameSkin);

            Plugin.Log.LogDebug($"Skin updated to: {gameSkin}");
        });
    }

    // Skin loader, hide bag, staff and hat
    public static void InitAppearance()
    {
        if (Singletons.NobetaSkin is not { } skin)
        {
            return;
        }

        if (skin.bagMesh is not null)
        {
            skin.bagMesh.enabled = !HideBagEnabled;
        }

        if (skin.weaponMesh is not null)
        {
            skin.weaponMesh.enabled = !HideStaffEnabled;
        }

        if (skin.storyHatMesh is not null)
        {
            skin.storyHatMesh.enabled = !HideHatEnabled;
        }

        // Second pass to remove from other skins
        foreach (var meshRenderer in skin.bodyMesh)
        {
            if (HideBagEnabled)
            {
                if (meshRenderer.name.Contains("bag", StringComparison.OrdinalIgnoreCase))
                {
                    meshRenderer.enabled = false;
                }
            }

            if (HideHatEnabled)
            {
                if (meshRenderer.name.Contains("hat", StringComparison.OrdinalIgnoreCase))
                {
                    meshRenderer.enabled = false;
                }
            }
        }
    }

    [HarmonyPatch(typeof(Game), nameof(Game.SwitchScene))]
    [HarmonyPrefix]
    private static void SwitchScenePrefix()
    {
        if (RandomizeSkin == BonusSettings.RandomSkin.Always)
        {
            SelectedSkin = (GameSkin) Random.Shared.Next(0, AvailableSkins.Length);
        }

        UpdateSelectedSkin();
    }

    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Update))]
    [HarmonyPostfix]
    private static void PlayerControllerUpdatePostfix()
    {
        InitAppearance();
    }
}