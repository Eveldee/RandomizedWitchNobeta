using RandomizedWitchNobeta.Shared;

namespace RandomizedWitchNobeta.Generation.Models;

public class InventoryState
{
    public SeedSettings SeedSettings { get; }

    public int ArcaneLevel { get; set; } = 0;
    public int IceLevel { get; set; } = 0;
    public int FireLevel { get; set; } = 0;
    public int ThunderLevel { get; set; } = 0;

    public int TokenAmount { get; set; } = 0;

    public int BossKilled { get; set; } = 0;

    public int ChestOpened { get; set; } = 0;

    public InventoryState(SeedSettings seedSettings)
    {
        SeedSettings = seedSettings;

        if (!seedSettings.NoArcane)
        {
            ArcaneLevel++;
        }
    }
}