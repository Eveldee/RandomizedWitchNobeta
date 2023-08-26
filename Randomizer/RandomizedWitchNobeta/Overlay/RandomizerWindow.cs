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
    private int _upgradeModeIndex = (int)SeedSettings.MagicUpgradeMode.Vanilla;
    private readonly string[] _availableUpgradeModes = Enums.GetNames<SeedSettings.MagicUpgradeMode>().Select(name => name.Humanize(LetterCasing.Title)).ToArray();

    private int _difficultyIndex = (int) GameDifficulty.Advanced;
    private readonly string[] _availableDifficulties = Enums.GetNames<GameDifficulty>().Select(name => name.Humanize(LetterCasing.Title)).ToArray();

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
                            _difficultyIndex = (int) settings.Difficulty;
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

                    if (ImGui.Combo("Difficulty", ref _difficultyIndex, _availableDifficulties,
                        _availableDifficulties.Length))
                    {
                        settings.Difficulty = (GameDifficulty) _difficultyIndex;
                    }
                    HelpMarker("Choose the game difficulty, for some reasons there is a 'Hard' difficulty in the game but it seems to be the exact same as 'Advanced'");

                    ImGui.NewLine();

                    ImGui.Checkbox("Random Start Level", ref settings.RandomStartLevel);
                    ImGui.Checkbox("Shuffle Exits", ref settings.ShuffleExits);

                    ImGui.NewLine();
                    ImGui.SeparatorText("Extra End Conditions");

                    ImGui.Checkbox("Magic Master", ref settings.MagicMaster);
                    HelpMarker("When enabled, it's needed to get all attack magics (arcane, ice, fire and thunder) to Lvl. Max (Lvl. 5) before being able to reach Nonota");
                    ImGui.Checkbox("Boss Hunt", ref settings.BossHunt);
                    HelpMarker("When enabled, it's needed to kill all bosses (including the one in Secret Passage, but not elites like the Seal) before being able to reach Nonota");

                    ImGui.NewLine();
                    ImGui.Checkbox("Trial Keys", ref settings.TrialKeys);
                    HelpMarker("Enabling this setting will add trial keys in the item pool. Each trial needs one key to be activated, so to reach Nonota it is needed to find at least 3 keys.");

                    WithDisabled(!settings.TrialKeys, () =>
                    {
                        ImGui.SliderInt("Trial Keys Amount", ref settings.TrialKeysAmount, 3, 7);
                    });

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

                // Update seed hash display
                Singletons.Dispatcher.Enqueue(() =>
                {
                    if (StartPatches.CopyrightText is { } copyrightText)
                    {
                        copyrightText.text =
                        $"""
                        Seed Hash: {settings.Hash():X8}
                        © 2022 Pupuya Games / SimonCreative / Justdan  © 2016 COVER Corp.
                        """;
                    }
                });
            });

            TabItem("Timers", ShowTimersConfigWindow);

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
                HelpMarker("'Once' will select a random skin at the start of a new run and keep it until the end of the run.\n'Always' will select a new random skin at each loading screen.");

                ImGui.NewLine();
                ImGui.Checkbox("Hide Bag", ref AppearancePatches.HideBagEnabled);

                ImGui.Checkbox("Hide Staff", ref AppearancePatches.HideStaffEnabled);

                ImGui.Checkbox("Hide Hat", ref AppearancePatches.HideHatEnabled);
            });
        });

        ImGui.End();
    }
}