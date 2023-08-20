using System.Collections.Generic;
using RandomizedWitchNobeta.Generation.Models;
using RandomizedWitchNobeta.Generation.Models.Requirements;

namespace RandomizedWitchNobeta.Generation;

public static class WorldGraph
{
    public const int LowestRegionNumber = 2;
    public const int HighestRegionNumber = 7;

    public static List<ItemSystem.ItemType> ItemPool => new()
    {
        // Magic - Key items
        ItemSystem.ItemType.MagicNull,
        ItemSystem.ItemType.MagicNull,
        ItemSystem.ItemType.MagicNull,
        ItemSystem.ItemType.MagicNull,
        ItemSystem.ItemType.MagicNull,

        ItemSystem.ItemType.MagicIce,
        ItemSystem.ItemType.MagicIce,
        ItemSystem.ItemType.MagicIce,
        ItemSystem.ItemType.MagicIce,
        ItemSystem.ItemType.MagicIce,

        ItemSystem.ItemType.MagicFire,
        ItemSystem.ItemType.MagicFire,
        ItemSystem.ItemType.MagicFire,
        ItemSystem.ItemType.MagicFire,
        ItemSystem.ItemType.MagicFire,

        ItemSystem.ItemType.MagicLightning,
        ItemSystem.ItemType.MagicLightning,
        ItemSystem.ItemType.MagicLightning,
        ItemSystem.ItemType.MagicLightning,
        ItemSystem.ItemType.MagicLightning,

        // Magic - Others
        ItemSystem.ItemType.Absorb,
        ItemSystem.ItemType.Absorb,
        ItemSystem.ItemType.Absorb,
        ItemSystem.ItemType.Absorb,

        ItemSystem.ItemType.SkyJump,
        ItemSystem.ItemType.SkyJump,
        ItemSystem.ItemType.SkyJump,
        ItemSystem.ItemType.SkyJump,

        // Bag upgrades
        ItemSystem.ItemType.BagMaxAdd,
        ItemSystem.ItemType.BagMaxAdd,
        ItemSystem.ItemType.BagMaxAdd,
        ItemSystem.ItemType.BagMaxAdd,

        // Fill with purples (TODO: add more items)
        ItemSystem.ItemType.MysteriousB,
        ItemSystem.ItemType.MysteriousB,
        ItemSystem.ItemType.MysteriousB,
        ItemSystem.ItemType.MysteriousB,
        ItemSystem.ItemType.MysteriousB,
        ItemSystem.ItemType.MysteriousB,
        ItemSystem.ItemType.MysteriousB,
        ItemSystem.ItemType.MysteriousB,
        ItemSystem.ItemType.MysteriousB,
        ItemSystem.ItemType.MysteriousB,
        ItemSystem.ItemType.MysteriousB,
        ItemSystem.ItemType.MysteriousB,
        ItemSystem.ItemType.MysteriousB
    };

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
                    ChestItem("TreasureBox_Room03"),
                    ChestItem("TreasureBox02_Room03")
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

                        Exits = new []
                        {
                            Exit(2, 3, -1),
                            Exit(2, 3, 2)
                        },

                        ItemLocations = new []
                        {
                            ChestItem("TreasureBox_Room05")
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

                                Exits = new []
                                {
                                    Exit(2, 5, 5)
                                },

                                ItemLocations = new []
                                {
                                    ChestItem("TreasureBox07"),
                                    ChestItem("TreasureBox07To08"),
                                    ChestItem("TreasureBox08"),
                                    ChestItem("TreasureBox09"),
                                    ChestItem("TreasureBox10")
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
                    ChestItem("TreasureBox_Room01"),
                    ChestItem("TreasureBox_Room03"),
                    ChestItem("TreasureBox_Room04"),
                    ChestItem("TreasureBox_Room05_01"),
                    ChestItem("TreasureBox_Room05_02")
                },

                NextRegion = new RegionTransition
                {
                    Requirement = new MagicRequirement(MagicType.Ice, 1),

                    Destination = new Region
                    {
                        FriendlyName = "Underground - Tania",

                        Exits = new []
                        {
                            Exit(3, 2, 2),
                            Exit(3, 4, 3),
                            Exit(3, 4, 2)
                        },

                        ItemLocations = new []
                        {
                            ChestItem("TreasureBox_Room08")
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
                    ChestItem("Room02_TreasureBox01"),
                    ChestItem("Room02_TreasureBox02"),
                    ChestItem("Room03_TreasureBox01"),
                    ChestItem("Room03_TreasureBox02"),
                    ChestItem("Room02To04_TreasureBox02"),
                    ChestItem("Room05To06_TreasureBox"),
                    ChestItem("Room06_TreasureBox")
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

                        Exits = new []
                        {
                            Exit(4, 3, 4),
                            Exit(4, 5, 3)
                        },

                        ItemLocations = new []
                        {
                            ChestItem("Room07_TreasureBox"),
                            ChestItem("Room08_TreasureBox"),
                            ChestItem("Room01_TreasureBox")
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
                    ChestItem("TreasureBox02_Room02_03"),
                    ChestItem("TreasureBox02_Room03_02"),
                    ChestItem("TreasureBox02_Room03_01"),
                    ChestItem("TreasureBox02_Room04"),
                    ChestItem("TreasureBox02_Room05")
                },

                NextRegion = new RegionTransition
                {
                    Requirement = new MagicRequirement(MagicType.Thunder, 1),

                    Destination = new Region
                    {
                        FriendlyName = "Dark Tunnel - After Thunder",

                        Exits = new []
                        {
                            Exit(5, 2, 5),
                            Exit(5, 6, -1)
                        },

                        ItemLocations = new []
                        {
                            ChestItem("TreasureBox02_Room06To07"),
                            ChestItem("TreasureBox02_Room07"),
                            ChestItem("TreasureBox02_Room08"),
                            ChestItem("TreasureBox02_Room09To10")
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
                    ChestItem("TreasureBox02_R02"),
                    ChestItem("TreasureBox02_R03"),
                    ChestItem("TreasureBox02_R0401")
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
                            ChestItem("TreasureBox02_R0402"),
                            ChestItem("TreasureBox02_R06"),
                            ChestItem("TreasureBox02_R07")
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

                                Exits = new []
                                {
                                    Exit(6, 7, -1)
                                },

                                ItemLocations = new []
                                {
                                    ChestItem("TreasureBox02_R08")
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
                    ChestItem("TreasureBox_Act02Room04"),
                    ChestItem("TreasureBox_Act02Room05")
                },

                NextRegion = new RegionTransition
                {
                    Requirement = new RequirementBuilder()
                        .Magic(MagicType.Fire, 1)
                        .Magic(MagicType.Thunder, 1)
                        .Build(),

                    Destination = new Region
                    {
                        FriendlyName = "Trials",

                        Exits = null,

                        ItemLocations = new []
                        {
                            ChestItem("Act04Room05To06_TreasureBox"),
                            ChestItem("Act05_TreasureBox02_Room09To10"),
                            ChestItem("Act03TreasureBox_Room05_02")
                        },

                        NextRegion = null,

                        FinalRegion = true
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

    private static ItemLocation ChestItem(string chestName)
    {
        var itemLocation = new ChestItemLocation(chestName);

        ItemLocations.Add(itemLocation);

        return itemLocation;
    }
}