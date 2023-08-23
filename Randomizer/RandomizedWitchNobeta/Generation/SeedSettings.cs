﻿using System;
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
    public bool RandomStartLevel = true;
    [JsonInclude]
    public bool ShuffleExits = true;
    [JsonInclude]
    public bool TrialKeys = true;
    [JsonInclude]
    public int TrialKeysAmount = 5;
    [JsonInclude]
    public bool NoArcane = true;
    [JsonInclude]
    public MagicUpgradeMode MagicUpgrade = MagicUpgradeMode.BossKill;

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
}