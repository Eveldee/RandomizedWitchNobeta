using System.Collections.Generic;
using RandomizedWitchNobeta.Generation.Models;
using RandomizedWitchNobeta.Generation.Models.Requirements;

namespace RandomizedWitchNobeta.Generation;

public class SeedGenerator
{
    private static List<ItemSystem.ItemType> ItemPool => new()
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

    private static readonly Dictionary<int, Region> Regions = new()
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
                            new RegionExit(3, -1),
                            new RegionExit(3, 2)
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
                                    new RegionExit(5, 5)
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
                            new RegionExit(2, 2),
                            new RegionExit(4, 3),
                            new RegionExit(4, 2)
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
                            new RegionExit(3, 4),
                            new RegionExit(5, 3)
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
                            new RegionExit(2, 5),
                            new RegionExit(6, -1)
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
                                    new RegionExit(7, -1)
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

    private static readonly List<ItemLocation> ItemLocations = new();

    private static ItemLocation ChestItem(string chestName)
    {
        var itemLocation = new ChestItemLocation(chestName);

        ItemLocations.Add(itemLocation);

        return itemLocation;
    }
}