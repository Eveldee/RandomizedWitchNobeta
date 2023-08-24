using ImGuiNET;
using RandomizedWitchNobeta.Timer;
using RandomizedWitchNobeta.Utils;
using UnityEngine;

namespace RandomizedWitchNobeta.Overlay;

public partial class NobetaRandomizerOverlay
{
    private void ShowTimersConfigWindow()
    {
        ImGui.TextColored(InfoColor, "Timers");

        if (Singletons.Timers is null)
        {
            ImGui.Text("Waiting for timers to be loaded...");
        }
        else
        {
            ImGui.SeparatorText("General");

            ImGui.Checkbox("Show Timers", ref Timers.ShowTimers);
            HelpMarker("This will pause timers on game pause (when opening the menu). Note that Real Time is unaffected as it shows time since game start");

            ImGui.SeparatorText("Style");
            ImGui.SliderFloat("X", ref _timersWindowPosition.X, _borderSize, Screen.width - _timersWindowSize.X - _borderSize);
            ImGui.SliderFloat("Y", ref _timersWindowPosition.Y, _borderSize, Screen.height - _timersWindowSize.Y - _borderSize);

            ImGui.DragFloat("Border", ref _borderSize, 1f, 0f, float.MaxValue);
            ImGui.DragFloat("Rounding", ref _borderRounding, 1f, 0f, float.MaxValue);

            ImGui.ColorEdit4("Text", ref _timersTextColor);
            ImGui.ColorEdit4("Background", ref _timersBackgroundColor);
            ImGui.ColorEdit4("Border", ref _timersBorderColor);

            ImGui.SeparatorText("Timers");

            ImGui.Checkbox("Real Time", ref Timers.ShowRealTime);
            ImGui.Checkbox("Load Removed Timer", ref Timers.ShowLoadRemovedTimer);
        }
    }
}