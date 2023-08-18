using System.Threading.Tasks;
using Il2CppInterop.Runtime;
using ImGuiNET;
using RandomizedWitchNobeta.Overlay;
using RandomizedWitchNobeta.Timer;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Overlay;

public partial class NobetaRandomizerOverlay : ClickableTransparentOverlay.Overlay
{
    public NobetaRandomizerOverlay() : base("RandomizedWitchNobeta")
    {

    }

    protected override Task PostInitialized()
    {
        VSync = true;

        IL2CPP.il2cpp_thread_attach(IL2CPP.il2cpp_domain_get());
        NobetaProcessUtils.OverlayWindowHandle = NobetaProcessUtils.FindWindow(null, "RandomizedWitchNobeta");
        NobetaProcessUtils.HideOverlayFromTaskbar();

        return Task.CompletedTask;
    }

    protected override void Render()
    {
        ImGui.ShowDemoWindow();
        // // Timers are always visible when activated, even if overlay is hidden
        // if (Timers.ShowTimers)
        // {
        //     ShowTimersWindow();
        // }
        //
        // if (!OverlayState.ShowOverlay)
        // {
        //     return;
        // }
        //
        // if (OverlayState.ShowOverlay)
        // {
        //     ShowTrainerWindow();
        // }
        // if (OverlayState.ShowTimersConfigWindow)
        // {
        //     ShowTimersConfigWindow();
        // }
    }
}