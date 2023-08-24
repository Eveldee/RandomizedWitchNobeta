using RandomizedWitchNobeta.Config;
using RandomizedWitchNobeta.Generation;

namespace RandomizedWitchNobeta.Overlay;

[Section("Overlay")]
public static class OverlayState
{
    public static bool ShowOverlay = false;

    public static readonly SeedSettings SeedSettings = new ();
}