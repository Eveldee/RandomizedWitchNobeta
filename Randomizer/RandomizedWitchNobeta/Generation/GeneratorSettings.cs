using System.Collections.Generic;

namespace RandomizedWitchNobeta.Generation;

public class SeedSettings
{
    public int Seed { get; set; }

    public int ChestSoulCount { get; set; } = 250;

    public Dictionary<int, int> StartSouls { get; set; } = new()
    {
        { 2, 0 },
        { 3, 60 },
        { 4, 500 },
        { 5, 900 },
        { 6, 1300 }
    };
}