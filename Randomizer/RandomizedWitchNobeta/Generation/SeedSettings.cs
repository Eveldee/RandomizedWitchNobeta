using System;
using System.Collections.Generic;
using System.IO.Hashing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using RandomizedWitchNobeta.Config.Serialization;

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
    public bool NoArcane = false;
    [JsonInclude]
    public MagicUpgradeMode MagicUpgrade = MagicUpgradeMode.Vanilla;

    [JsonInclude]
    public bool BossHunt = false;
    [JsonInclude]
    public bool MagicMaster = false;

    [JsonInclude]
    public bool TrialKeys = false;
    [JsonInclude]
    public int TrialKeysAmount = 5;

    [JsonInclude]
    public bool OneHitKO = false;
    [JsonInclude]
    public bool DoubleDamage = false;
    [JsonInclude]
    public bool HalfDamage = false;

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

        NoArcane = other.NoArcane;
        MagicUpgrade = other.MagicUpgrade;

        BossHunt = other.BossHunt;
        MagicMaster = other.MagicMaster;

        TrialKeys = other.TrialKeys;
        TrialKeysAmount = other.TrialKeysAmount;

        OneHitKO = other.OneHitKO;
        DoubleDamage = other.DoubleDamage;
        HalfDamage = other.HalfDamage;

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
        // This is not ideal but it gives consistent results
        return BitConverter.ToInt32(Crc32.Hash(Encoding.UTF8.GetBytes(SerializeUtils.SerializeIndented(this))));
    }
}