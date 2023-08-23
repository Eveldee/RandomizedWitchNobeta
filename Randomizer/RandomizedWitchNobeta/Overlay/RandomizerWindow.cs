using System.Linq;
using System.Numerics;
using Humanizer;
using ImGuiNET;
using RandomizedWitchNobeta.Bonus;
using RandomizedWitchNobeta.Utils;

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