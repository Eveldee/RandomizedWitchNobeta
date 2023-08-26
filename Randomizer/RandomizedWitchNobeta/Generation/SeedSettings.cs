using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RandomizedWitchNobeta.Generation;

// Need to use fields here for ImGui]
public class SeedSettings
{
    public enum MagicUpgradeMode
    {
        Vanilla,
        BossKill
    }

    [JsonInclude]
    public int Seed = Random.Shared.Next();

    [JsonInclude]
    public GameDifficulty Difficulty = GameDifficulty.Advanced;

    [JsonInclude]
    public bool RandomStartLevel = true;
    [JsonInclude]
    public bool ShuffleExits = true;
    [JsonInclude]
    public bool TrialKeys = false;
    [JsonInclude]
    public int TrialKeysAmount = 5;
    [JsonInclude]
    public bool NoArcane = false;
    [JsonInclude]
    public MagicUpgradeMode MagicUpgrade = MagicUpgradeMode.Vanilla;

    [JsonInclude]
    public int ChestSoulCount = 250;

    [JsonInclude]
    public float StartSoulsModifier = 1f;
    public Dictionary<int, int> StartSouls { get; set; } = new()
    {
        { 2, 0 },
        { 3, 60 },
        { 4, 500 },
        { 5, 900 },
        { 6, 1300 }
    };

    // Weights
    [JsonInclude]
    public int ItemWeightSouls = 3;
    [JsonInclude]
    public int ItemWeightHP = 1;
    [JsonInclude]
    public int ItemWeightMP = 1;
    [JsonInclude]
    public int ItemWeightDefense = 1;
    [JsonInclude]
    public int ItemWeightHoly = 1;
    [JsonInclude]
    public int ItemWeightArcane = 2;

    public void Apply(SeedSettings other)
    {
        Seed = other.Seed;

        Difficulty = other.Difficulty;

        RandomStartLevel = other.RandomStartLevel;
        ShuffleExits = other.ShuffleExits;

        TrialKeys = other.TrialKeys;
        TrialKeysAmount = other.TrialKeysAmount;

        NoArcane = other.NoArcane;
        MagicUpgrade = other.MagicUpgrade;

        ChestSoulCount = other.ChestSoulCount;
        StartSoulsModifier = other.StartSoulsModifier;
        StartSouls = other.StartSouls;

        ItemWeightSouls = other.ItemWeightSouls;
        ItemWeightHP = other.ItemWeightHP;
        ItemWeightMP = other.ItemWeightMP;
        ItemWeightDefense = other.ItemWeightDefense;
        ItemWeightHoly = other.ItemWeightHoly;
        ItemWeightArcane = other.ItemWeightArcane;
    }

    public int Hash()
    {
        var hashCode = new HashCode();

        // Add version to make sure a seed is valid only for a specific version of the same randomizer
        hashCode.Add(MyPluginInfo.PLUGIN_VERSION);

        hashCode.Add(Seed);
        hashCode.Add(Difficulty);
        hashCode.Add(RandomStartLevel);
        hashCode.Add(ShuffleExits);
        hashCode.Add(TrialKeys);
        hashCode.Add(TrialKeysAmount);
        hashCode.Add(NoArcane);
        hashCode.Add((int) MagicUpgrade);
        hashCode.Add(ChestSoulCount);
        hashCode.Add(StartSoulsModifier);
        hashCode.Add(StartSouls);
        hashCode.Add(ItemWeightSouls);
        hashCode.Add(ItemWeightHP);
        hashCode.Add(ItemWeightMP);
        hashCode.Add(ItemWeightDefense);
        hashCode.Add(ItemWeightHoly);
        hashCode.Add(ItemWeightArcane);

        return hashCode.ToHashCode();
    }
}