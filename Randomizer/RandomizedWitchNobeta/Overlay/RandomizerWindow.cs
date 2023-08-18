using System.Linq;
using System.Numerics;
using Humanizer;
using ImGuiNET;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Overlay;

public partial class NobetaRandomizerOverlay
{
    private void ShowTrainerWindow()
    {
        ImGui.Begin("Randomized Witch Nobeta", ref OverlayState.ShowOverlay);

        ImGui.TextColored(TitleColor, $"Welcome to Randomized Witch Nobeta v{MyPluginInfo.PLUGIN_VERSION}");

        ImGui.End();
    }
}