using System;
using System.Collections.Generic;

namespace RandomizedWitchNobeta.Generation;

// Need to use fields here for ImGui
public class SeedSettings
{
    public int Seed = Random.Shared.Next();

    public int ChestSoulCount = 250;

    public Dictionary<int, int> StartSouls { get; set; } = new()
    {
        { 2, 0 },
        { 3, 60 },
        { 4, 500 },
        { 5, 900 },
        { 6, 1300 }
    };
}