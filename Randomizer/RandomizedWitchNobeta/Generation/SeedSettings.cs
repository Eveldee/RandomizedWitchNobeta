using System;
using System.Collections.Generic;
using System.IO.Hashing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using MessagePack;
using RandomizedWitchNobeta.Config.Serialization;

namespace RandomizedWitchNobeta.Generation;

// Need to use fields here for ImGui
[MessagePackObject]
public class SeedSettings
{
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
    public int Seed = Random.Shared.Next();

    [JsonInclude, Key(1)]
    public GameDifficulty Difficulty = GameDifficulty.Advanced;

    [JsonInclude, Key(2)]
    public StartLevelSetting StartLevel = StartLevelSetting.Random;
    [JsonInclude, Key(3)]
    public bool ShuffleExits = true;

    [JsonInclude, Key(4)]
    public bool NoArcane = false;
    [JsonInclude, Key(5)]
    public MagicUpgradeMode MagicUpgrade = MagicUpgradeMode.Vanilla;
    [JsonInclude, Key(21)]
    public int BookAmount = 1;

    [JsonInclude, Key(6)]
    public bool BossHunt = false;
    [JsonInclude, Key(7)]
    public bool MagicMaster = false;

    [JsonInclude, Key(8)]
    public bool TrialKeys = false;
    [JsonInclude, Key(9)]
    public int TrialKeysAmount = 5;

    [JsonInclude, Key(10)]
    public bool OneHitKO = false;
    [JsonInclude, Key(11)]
    public bool DoubleDamage = false;
    [JsonInclude, Key(12)]
    public bool HalfDamage = false;

    [JsonInclude, Key(13)]
    public int ChestSoulCount = 250;

    [JsonInclude, Key(14)]
    public float StartSoulsModifier = 1f;

    // Weights
    [JsonInclude, Key(15)]
    public int ItemWeightSouls = 3;
    [JsonInclude, Key(16)]
    public int ItemWeightHP = 1;
    [JsonInclude, Key(17)]
    public int ItemWeightMP = 1;
    [JsonInclude, Key(18)]
    public int ItemWeightDefense = 1;
    [JsonInclude, Key(19)]
    public int ItemWeightHoly = 1;
    [JsonInclude, Key(20)]
    public int ItemWeightArcane = 2;

    public void Apply(SeedSettings other)
    {
        Seed = other.Seed;

        Difficulty = other.Difficulty;

        StartLevel = other.StartLevel;
        ShuffleExits = other.ShuffleExits;

        NoArcane = other.NoArcane;
        MagicUpgrade = other.MagicUpgrade;
        BookAmount = other.BookAmount;

        BossHunt = other.BossHunt;
        MagicMaster = other.MagicMaster;

        TrialKeys = other.TrialKeys;
        TrialKeysAmount = other.TrialKeysAmount;

        OneHitKO = other.OneHitKO;
        DoubleDamage = other.DoubleDamage;
        HalfDamage = other.HalfDamage;

        ChestSoulCount = other.ChestSoulCount;
        StartSoulsModifier = other.StartSoulsModifier;

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