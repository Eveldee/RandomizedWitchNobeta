using System;
using System.Linq;
using System.Numerics;
using Humanizer;
using ImGuiNET;
using RandomizedWitchNobeta.Bonus;
using RandomizedWitchNobeta.Config.Serialization;
using RandomizedWitchNobeta.Utils;
using TextCopy;

namespace RandomizedWitchNobeta.Overlay;

public partial class NobetaRandomizerOverlay
{
    private void ShowRandomizerWindow()
    {
        ImGui.Begin("Randomized Witch Nobeta");

        ImGui.TextColored(TitleColor, $"Welcome to Randomized Witch Nobeta v{MyPluginInfo.PLUGIN_VERSION}");

        TabBar("MainTabBar", () =>
        {
            TabItem("Randomizer", () =>
            {
                var settings = OverlayState.SeedSettings;

                if (ImGui.CollapsingHeader("Import/Export", ImGuiTreeNodeFlags.DefaultOpen))
                {
                    ImGui.SeparatorText("");

                    if (ButtonColored(InfoColor, "Export to Clipboard"))
                    {
                        SettingsExporter.ExportSettings(settings);
                    }

                    ImGui.SameLine();
                    if (ButtonColored(ValueColor, "Import from Clipboard"))
                    {
                        if (SettingsExporter.TryImportSettings(out var imported))
                        {
                            // Apply settings to keep references
                            settings.Seed = imported.Seed;
                            settings.ChestSoulCount = imported.ChestSoulCount;
                            settings.StartSouls = imported.StartSouls;
                        }
                    }

                    ImGui.SeparatorText("");
                }

                if (ImGui.CollapsingHeader("Seed Settings", ImGuiTreeNodeFlags.DefaultOpen))
                {
                    ImGui.SeparatorText("General");

                    if (ImGui.Button(" Random ##Seed"))
                    {
                        settings.Seed = Random.Shared.Next();
                    }

                    ImGui.SameLine();
                    ImGui.InputInt("Seed", ref settings.Seed);

                    ImGui.SeparatorText("Balance");

                    ImGui.InputInt("Souls in checks", ref settings.ChestSoulCount, 50);

                    ImGui.SeparatorText("Item Pool");
                }
            });

            TabItem("Bonus", () =>
            {
                ImGui.SeparatorText("");

                ImGui.PushTextWrapPos();
                ImGui.TextColored(InfoColor, "It is possible to do some minor tweaks that does not impact gameplay here.");
                ImGui.PopTextWrapPos();

                ImGui.SeparatorText("Skin");

                if (ImGui.Combo("Selected skin", ref AppearancePatches.SelectedSkinIndex, AppearancePatches.AvailableSkins,
                    AppearancePatches.AvailableSkins.Length))
                {
                    AppearancePatches.UpdateSelectedSkin();
                }

                ImGui.NewLine();

                ImGui.AlignTextToFramePadding();
                ImGui.Text("Randomize skin");
                ImGui.SameLine();
                ImGui.RadioButton("No", ref AppearancePatches.RandomizeSkin, AppearancePatches.RandomizeSkin_No);
                ImGui.SameLine();
                ImGui.RadioButton("Once", ref AppearancePatches.RandomizeSkin, AppearancePatches.RandomizeSkin_Once);
                ImGui.SameLine();
                ImGui.RadioButton("Always", ref AppearancePatches.RandomizeSkin, AppearancePatches.RandomizeSkin_Always);

                ImGui.NewLine();
                ImGui.Checkbox("Hide Bag", ref AppearancePatches.HideBagEnabled);

                ImGui.Checkbox("Hide Staff", ref AppearancePatches.HideStaffEnabled);

                ImGui.Checkbox("Hide Hat", ref AppearancePatches.HideHatEnabled);
            });
        });

        ImGui.End();
    }
}