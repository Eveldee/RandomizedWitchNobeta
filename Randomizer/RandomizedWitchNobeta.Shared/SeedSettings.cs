using System.IO.Hashing;
using System.Text;
using System.Text.Json.Serialization;
using MessagePack;

namespace RandomizedWitchNobeta.Shared;

[MessagePackObject]
public class SeedSettings
{
    public enum GameDifficulty
    {
        Standard,
        Advanced,
        Hard,
        BossRush
    }

    public enum MagicUpgradeMode
    {
        Vanilla,
        BossKill
    }

    public enum StartLevelSetting
    {
        Random,
        OkunShrine,
        UndergroundCave,
        LavaRuins,
        DarkTunnel,
        SpiritRealm
    }

    [JsonInclude, Key(0)]
    public int Seed { get; set; } = Random.Shared.Next();

    [JsonInclude, Key(1)]
    public GameDifficulty Difficulty { get; set; } = GameDifficulty.Advanced;

    [JsonInclude, Key(2)]
    public StartLevelSetting StartLevel { get; set; } = StartLevelSetting.Random;
    [JsonInclude, Key(3)]
    public bool ShuffleExits { get; set; } = true;

    [JsonInclude, Key(22)]
    public bool GameHints { get; set; } = true;

    [JsonInclude, Key(4)]
    public bool NoArcane { get; set; } = false;
    [JsonInclude, Key(5)]
    public MagicUpgradeMode MagicUpgrade { get; set; } = MagicUpgradeMode.Vanilla;
    [JsonInclude, Key(21)]
    public int BookAmount { get; set; } = 1;

    [JsonInclude, Key(6)]
    public bool BossHunt { get; set; } = false;
    [JsonInclude, Key(7)]
    public bool MagicMaster { get; set; } = false;

    [JsonInclude, Key(8)]
    public bool TrialKeys { get; set; } = false;
    [JsonInclude, Key(9)]
    public int TrialKeysAmount { get; set; } = 5;

    [JsonInclude, Key(10)]
    public bool OneHitKO { get; set; } = false;
    [JsonInclude, Key(11)]
    public bool DoubleDamage { get; set; } = false;
    [JsonInclude, Key(12)]
    public bool HalfDamage { get; set; } = false;

    [JsonInclude, Key(13)]
    public int ChestSoulCount { get; set; } = 250;

    [JsonInclude, Key(14)]
    public float StartSoulsModifier { get; set; } = 1f;

    // Weights
    [JsonInclude, Key(15)]
    public int ItemWeightSouls { get; set; } = 3;
    [JsonInclude, Key(16)]
    public int ItemWeightHP { get; set; } = 1;
    [JsonInclude, Key(17)]
    public int ItemWeightMP { get; set; } = 1;
    [JsonInclude, Key(18)]
    public int ItemWeightDefense { get; set; } = 1;
    [JsonInclude, Key(19)]
    public int ItemWeightHoly { get; set; } = 1;
    [JsonInclude, Key(20)]
    public int ItemWeightArcane { get; set; } = 2;

    public int Hash()
    {
        // This is not ideal but it gives consistent results
        return BitConverter.ToInt32(Crc32.Hash(Encoding.UTF8.GetBytes(SerializeUtils.SerializeIndented(this))));
    }
}