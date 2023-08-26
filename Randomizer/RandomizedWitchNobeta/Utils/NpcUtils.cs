using System.Collections.Generic;

namespace RandomizedWitchNobeta.Utils;

public static class NpcUtils
{
    public static readonly HashSet<string> ValidBosses = new()
    {
        "Boss_Act01", // Armor
        "Boss_Level02", // Tania
        "Boss_Level03_Big", // Monica
        "Boss_Level04", // Vanessa
        "Boss_Level05", // Vanessa 2
        "Boss_Act01_Plus" // Armor in Secret Passage
    };
}