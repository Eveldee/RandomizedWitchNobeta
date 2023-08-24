using System.Numerics;
using ImGuiNET;
using RandomizedWitchNobeta.Config;
using RandomizedWitchNobeta.Utils;
using static RandomizedWitchNobeta.Timer.Timers;

namespace RandomizedWitchNobeta.Overlay;

[Section("Timers.Overlay")]
public partial class NobetaRandomizerOverlay
{
    private Vector2 _timersWindowSize = new(0, 0);
    [Bind]
    private static Vector2 _timersWindowPosition = new(1, 1);

    [Bind]
    private static Vector4 _timersBackgroundColor = new(50 / 255f, 50 / 255f, 70 / 255f, .9f);
    [Bind]
    private static Vector4 _timersBorderColor = new(250 / 255f, 250 / 255f, 255 / 255f, .7f);
    [Bind]
    private static Vector4 _timersTextColor = new(255 / 255f, 255 / 255f, 255 / 255f, 1f);

    [Bind]
    private static float _borderSize = 1f;
    [Bind]
    private static float _borderRounding = 0f;

    protected void ShowTimersWindow()
    {
        ImGui.PushStyleColor(ImGuiCol.WindowBg, _timersBackgroundColor);
        ImGui.PushStyleColor(ImGuiCol.Border, _timersBorderColor);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, _borderSize);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, _borderRounding);

        ImGui.Begin("Timers", ref ShowTimers, ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoInputs);

        ImGui.SetWindowSize(new Vector2(0, 0));
        ImGui.SetWindowPos(_timersWindowPosition);


        if (Singletons.RuntimeVariables is not { } runtimeVariables)
        {
            ImGui.Text("Waiting for timers to be loaded...");
        }
        else
        {
            if (ShowRealTime)
            {
                ImGui.TextColored(_timersTextColor, $"Real Time    : {runtimeVariables.ElapsedRealTime.ToString(FormatUtils.TimeSpanMillisFormat)}");
            }
            if (ShowLoadRemovedTimer)
            {
                ImGui.TextColored(_timersTextColor, $"Load Removed : {runtimeVariables.ElapsedLoadRemoved.ToString(FormatUtils.TimeSpanMillisFormat)}");
            }
        }

        _timersWindowSize = ImGui.GetWindowSize();

        ImGui.End();

        ImGui.PopStyleVar(2);
        ImGui.PopStyleColor(2);
    }
}