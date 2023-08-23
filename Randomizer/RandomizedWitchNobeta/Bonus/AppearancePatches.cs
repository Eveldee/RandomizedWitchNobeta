using System;
using System.Diagnostics;
using System.Linq;
using EnumsNET;
using HarmonyLib;
using RandomizedWitchNobeta.Config;
using RandomizedWitchNobeta.Utils;
using UnityEngine;
using Random = System.Random;

namespace RandomizedWitchNobeta.Bonus;

[Section("Bonus.Appearance")]
public static class AppearancePatches
{
    [Bind]
    public static int SelectedSkinIndex;
    public static readonly string[] AvailableSkins = Enum.GetNames<GameSkin>();

    [Bind]
    public static bool HideBagEnabled;
    [Bind]
    public static bool HideStaffEnabled;
    [Bind]
    public static bool HideHatEnabled;

    [Bind]
    public static int RandomizeSkin;

    public const int RandomizeSkin_No = 0;
    public const int RandomizeSkin_Once = 1;
    public const int RandomizeSkin_Always = 2;

    public static void UpdateSelectedSkin()
    {
        var gameSkin = (GameSkin) SelectedSkinIndex;

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
        if (RandomizeSkin == RandomizeSkin_Always)
        {
            var gameSkin = (GameSkin)Random.Shared.Next(0, AvailableSkins.Length);

            Game.Collection.UpdateSkin(gameSkin);
        }
    }

    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Update))]
    [HarmonyPostfix]
    private static void PlayerControllerUpdatePostfix()
    {
        InitAppearance();
    }
}