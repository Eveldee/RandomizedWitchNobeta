using System.Collections.Generic;
using RandomizedWitchNobeta.Generation.Models;
using RandomizedWitchNobeta.Generation.Models.Requirements;

namespace RandomizedWitchNobeta.Generation;

public static class WorldGraph
{
    public const int LowestRegionNumber = 2;
    public const int HighestRegionNumber = 7;

    public const int ChestsCount = 45;

    public static readonly List<RegionExit> Exits = new();

    public static readonly List<ItemLocation> ItemLocations = new();

    public static readonly Dictionary<int, Region> Regions = new()
    {
        { 2, new Region
            {
                FriendlyName = "Shrine - Start",

                Exits = null,

                ItemLocations = new []
                {
                    ChestItem("TreasureBox_Room03", 2),
                    ChestItem("TreasureBox02_Room03", 2)
                },

                NextRegion = new RegionTransition
                {
                    Requirement = new RequirementBuilder()
                        .Magic(MagicType.Arcane, 1)
                        .Magic(MagicType.Thunder, 1)
                        .Build(),

                    Destination = new Region
                    {
                        FriendlyName = "Shrine - Armor Hall",
                        ContainsBoss = true,

                        Exits = new []
                        {
                            Exit(2, 3, -1),
                            Exit(2, 3, 2)
                        },

                        ItemLocations = new []
                        {
                            ChestItem("TreasureBox_Room05", 2)
                        },

                        NextRegion = new RegionTransition
                        {
                            Requirement = new RequirementBuilder()
                                .Magic(MagicType.Fire, 1)
                                .Magic(MagicType.Thunder, 1)
                                .Build(),

                            Destination = new Region
                            {
                                FriendlyName = "Shrine - Secret Passage",
                                ContainsBoss = true,

                                Exits = new []
                                {
                                    Exit(2, 5, 5)
                                },

                                ItemLocations = new []
                                {
                                    ChestItem("TreasureBox07", 2),
                                    ChestItem("TreasureBox07To08", 2),
                                    ChestItem("TreasureBox08", 2),
                                    ChestItem("TreasureBox09", 2),
                                    ChestItem("TreasureBox10", 2)
                                },

                                NextRegion = null
                            }
                        }
                    }
                }
            }
        },
        { 3, new Region
            {
                FriendlyName = "Underground - Start",

                Exits = null,

                ItemLocations = new []
                {
                    ChestItem("TreasureBox_Room01", 3),
                    ChestItem("TreasureBox_Room03", 3),
                    ChestItem("TreasureBox_Room04", 3),
                    ChestItem("TreasureBox_Room05_01", 3),
                    ChestItem("TreasureBox_Room05_02", 3),
                    CatItem()
                },

                NextRegion = new RegionTransition
                {
                    Requirement = new MagicRequirement(MagicType.Ice, 1),

                    Destination = new Region
                    {
                        FriendlyName = "Underground - Tania",
                        ContainsBoss = true,

                        Exits = new []
                        {
                            Exit(3, 2, 2),
                            Exit(3, 4, 3),
                            Exit(3, 4, 2)
                        },

                        ItemLocations = new []
                        {
                            ChestItem("TreasureBox_Room08", 3)
                        },

                        NextRegion = null
                    }
                }
            }
        },
        { 4, new Region
            {
                FriendlyName = "Lava Ruins - Start",

                Exits = null,

                ItemLocations = new []
                {
                    ChestItem("Room02_TreasureBox01", 4),
                    ChestItem("Room02_TreasureBox02", 4),
                    ChestItem("Room03_TreasureBox01", 4),
                    ChestItem("Room03_TreasureBox02", 4),
                    ChestItem("Room02To04_TreasureBox02", 4),
                    ChestItem("Room05To06_TreasureBox", 4),
                    ChestItem("Room06_TreasureBox", 4)
                },

                NextRegion = new RegionTransition
                {
                    Requirement = new RequirementBuilder()
                        .Magic(MagicType.Fire, 1)
                        .Magic(MagicType.Thunder, 1)
                        .Build(),

                    Destination = new Region
                    {
                        FriendlyName = "Lava Ruins - After Fire Barrier",
                        ContainsBoss = true,

                        Exits = new []
                        {
                            Exit(4, 3, 4),
                            Exit(4, 5, 3)
                        },

                        ItemLocations = new []
                        {
                            ChestItem("Room07_TreasureBox", 4),
                            ChestItem("Room08_TreasureBox", 4),
                            ChestItem("Room01_TreasureBox", 4)
                        },

                        NextRegion = null
                    }
                }
            }
        },
        {
            5, new Region
            {
                FriendlyName = "Dark Tunnel - Start",

                Exits = null,

                ItemLocations = new []
                {
                    ChestItem("TreasureBox02_Room02_03", 5),
                    ChestItem("TreasureBox02_Room03_02", 5),
                    ChestItem("TreasureBox02_Room03_01", 5),
                    ChestItem("TreasureBox02_Room04", 5),
                    ChestItem("TreasureBox02_Room05", 5)
                },

                NextRegion = new RegionTransition
                {
                    Requirement = new MagicRequirement(MagicType.Thunder, 1),

                    Destination = new Region
                    {
                        FriendlyName = "Dark Tunnel - After Thunder",
                        ContainsBoss = true,

                        Exits = new []
                        {
                            Exit(5, 2, 5),
                            Exit(5, 6, -1)
                        },

                        ItemLocations = new []
                        {
                            ChestItem("TreasureBox02_Room06To07", 5),
                            ChestItem("TreasureBox02_Room07", 5),
                            ChestItem("TreasureBox02_Room08", 5),
                            ChestItem("TreasureBox02_Room09To10", 5)
                        },

                        NextRegion = null
                    }
                }
            }
        },
        {
            6, new Region
            {
                FriendlyName = "Spirit Realm - Start",

                Exits = null,

                ItemLocations = new []
                {
                    ChestItem("TreasureBox02_R02", 6),
                    ChestItem("TreasureBox02_R03", 6),
                    ChestItem("TreasureBox02_R0401", 6)
                },

                NextRegion = new RegionTransition
                {
                    Requirement = new MagicRequirement(MagicType.Arcane, 1),

                    Destination = new Region
                    {
                        FriendlyName = "Spirit Realm - After Arcane Barrier",

                        Exits = null,

                        ItemLocations = new []
                        {
                            ChestItem("TreasureBox02_R0402", 6),
                            ChestItem("TreasureBox02_R06", 6),
                            ChestItem("TreasureBox02_R07", 6)
                        },

                        NextRegion = new RegionTransition
                        {
                            Requirement = new RequirementBuilder(CombineMethod.All)
                                .Magic(MagicType.Ice, 1)
                                .Magic(MagicType.Thunder, 1)
                                .Build(),

                            Destination = new Region
                            {
                                FriendlyName = "Spirit Realm - After Teleport",
                                ContainsBoss = true,

                                Exits = new []
                                {
                                    Exit(6, 7, -1)
                                },

                                ItemLocations = new []
                                {
                                    ChestItem("TreasureBox02_R08", 6)
                                },

                                NextRegion = null
                            }
                        }
                    }
                }
            }
        },
        {
            7, new Region
            {
                FriendlyName = "Abyss",

                Exits = null,

                ItemLocations = new []
                {
                    ChestItem("TreasureBox_Act02Room04", 7),
                    ChestItem("TreasureBox_Act02Room05", 7)
                },

                NextRegion = new RegionTransition
                {
                    Requirement = new RequirementBuilder(CombineMethod.All)
                        .Token(3)
                        .OneOf(builder => builder
                            .Magic(MagicType.Fire, 1)
                            .Magic(MagicType.Thunder, 1)
                        ).Build(),

                    Destination = new Region
                    {
                        FriendlyName = "Trials",
                        FinalRegion = true,

                        Exits = null,

                        ItemLocations = new []
                        {
                            ChestItem("Act04Room05To06_TreasureBox", 7),
                            ChestItem("Act05_TreasureBox02_Room09To10", 7),
                            ChestItem("Act03TreasureBox_Room05_02", 7)
                        },

                        NextRegion = null
                    }
                }
            }
        }
    };

    private static RegionExit Exit(int sourceScene, int nextSceneNumber, int nextSavePoint)
    {
        var exit = new RegionExit(sourceScene, nextSceneNumber, nextSavePoint);

        Exits.Add(exit);

        return exit;
    }

    private static ItemLocation ChestItem(string chestName, int sceneNumber)
    {
        var itemLocation = new ChestItemLocation(chestName, sceneNumber);

        ItemLocations.Add(itemLocation);

        return itemLocation;
    }

    private static ItemLocation CatItem()
    {
        var itemLocation = new CatItemLocation();

        ItemLocations.Add(itemLocation);

        return itemLocation;
    }
}