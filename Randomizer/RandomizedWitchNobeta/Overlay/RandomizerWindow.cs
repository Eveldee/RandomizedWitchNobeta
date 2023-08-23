using System;
using System.Linq;
using EnumsNET;
using Humanizer;
using ImGuiNET;
using RandomizedWitchNobeta.Bonus;
using RandomizedWitchNobeta.Generation;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Overlay;

public partial class NobetaRandomizerOverlay
{
    private int _upgradeModeIndex = (int)SeedSettings.MagicUpgradeMode.BossKill;
    private readonly string[] _availableUpgradeModes = Enums.GetNames<SeedSettings.MagicUpgradeMode>().Select(name => name.Humanize(LetterCasing.Title)).ToArray();

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
                    if (ButtonColored(PrimaryButtonColor, "Import from Clipboard"))
                    {
                        if (SettingsExporter.TryImportSettings(out var imported))
                        {
                            // Apply settings to keep references
                            settings.Apply(imported);

                            // Also reflect changes to ui
                            _upgradeModeIndex = (int) settings.MagicUpgrade;
                        }
                    }

                    ImGui.SeparatorText("");
                }

                if (ImGui.CollapsingHeader("Seed Settings", ImGuiTreeNodeFlags.DefaultOpen))
                {
                    if (ImGui.Button("Reset Default"))
                    {
                        settings.Apply(new SeedSettings());
                    }

                    ImGui.SeparatorText("General");

                    if (ImGui.Button(" Random ##Seed"))
                    {
                        settings.Seed = Random.Shared.Next();
                    }
                    ImGui.SameLine();
                    ImGui.InputInt("Seed", ref settings.Seed);

                    ImGui.NewLine();

                    ImGui.Checkbox("Random Start Level", ref settings.RandomStartLevel);
                    ImGui.Checkbox("Shuffle Exits", ref settings.ShuffleExits);

                    ImGui.NewLine();
                    ImGui.SeparatorText("Magic");
                    if (ImGui.Combo("Upgrade Mode", ref _upgradeModeIndex, _availableUpgradeModes,
                            _availableUpgradeModes.Length))
                    {
                        settings.MagicUpgrade = (SeedSettings.MagicUpgradeMode)_upgradeModeIndex;
                    }

                    ImGui.Checkbox("No Arcane", ref settings.NoArcane);
                    HelpMarker("When this setting is enabled, Arcane magic will not be usable until it is found like any other magic.");

                    ImGui.NewLine();
                    ImGui.SeparatorText("Trial Keys");

                    ImGui.Checkbox("Enable Trial Keys", ref settings.TrialKeys);
                    HelpMarker("Enabling this setting will add trial keys in the item pool. Each trial needs one key to be activated, so to reach Nonota it is needed to find at least 3 keys.");

                    WithDisabled(!settings.TrialKeys, () =>
                    {
                        ImGui.SliderInt("Trial Keys Amount", ref settings.TrialKeysAmount, 3, 7);
                    });

                    ImGui.NewLine();
                    ImGui.SeparatorText("Balance");

                    ImGui.InputInt("Souls in checks", ref settings.ChestSoulCount, 50);
                    HelpMarker("This is the amount of souls given for checks containing souls (chests, cat, ...). The souls in the item pool for checks can be disabled by setting a weight of 0 in 'Item Pool Weights' below for Souls.");

                    ImGui.InputFloat("Start Souls Modifier", ref settings.StartSoulsModifier, 0.1f, 0.2f, "%.2f");
                    HelpMarker("The amount of souls given scaling on start level will be multiplied by this modifier. So for example, if you set 0.5, only half of the souls will be given. 0 means no souls at all at the start.");

                    ImGui.NewLine();
                    ImGui.SeparatorText("Item Pool Weights");

                    ImGui.SliderInt("Souls", ref settings.ItemWeightSouls, 0, 10);
                    ImGui.SliderInt("HP", ref settings.ItemWeightHP, 0, 10);
                    ImGui.SliderInt("MP", ref settings.ItemWeightMP, 0, 10);
                    ImGui.SliderInt("Defense", ref settings.ItemWeightDefense, 0, 10);
                    ImGui.SliderInt("Holy", ref settings.ItemWeightHoly, 0, 10);
                    ImGui.SliderInt("Arcane", ref settings.ItemWeightArcane, 0, 10);
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