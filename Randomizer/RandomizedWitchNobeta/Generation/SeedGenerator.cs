using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LibCpp2IL;
using RandomizedWitchNobeta.Features;
using RandomizedWitchNobeta.Generation.Models;
using RandomizedWitchNobeta.Shared;
using RandomizedWitchNobeta.Utils;
using RandomizedWitchNobeta.Utils.Nobeta;

namespace RandomizedWitchNobeta.Generation;

public class SeedGenerator
{
    private readonly SeedSettings _settings;

    private int _startRegion;
    private Dictionary<RegionExit, int> _exitsOverrides;
    private readonly List<ItemLocation> _itemLocations;

    public SeedGenerator(SeedSettings settings)
    {
        _settings = settings;

        _itemLocations = WorldGraph.ItemLocations;
    }

    public void Generate()
    {
        var random = new Random(_settings.Seed);

        Plugin.Log.LogMessage("Generating a seed...");
        int tries = 0;
        var stopWatch = Stopwatch.StartNew();

        // Generate item pool
        var itemPoolGenerator = new ItemPoolGenerator(_settings, random);

        do
        {
            tries++;

            GenerateExits(random);

            RandomFillItems(random, itemPoolGenerator);
        } while (!CheckCompletable());

        stopWatch.Stop();
        Plugin.Log.LogMessage($"A completable seed has been successfully generated in {tries} tries in {stopWatch.Elapsed.TotalSeconds} seconds!");

        // Generate runtime variables and store them
        Singletons.RuntimeVariables = new RuntimeVariables(_settings, _startRegion, _exitsOverrides, _itemLocations);
    }

    private void GenerateExits(Random random)
    {
        // Generate exits overrides, make it so a region never loop on itself
        _exitsOverrides = new Dictionary<RegionExit, int>();

        foreach (var regionExit in WorldGraph.Exits)
        {
            int newDestination;
            do
            {
                newDestination = _settings.ShuffleExits
                    ? random.Next(WorldGraph.LowestRegionNumber, WorldGraph.HighestRegionNumber + 1)
                    : regionExit.NextSceneNumber;

                _exitsOverrides[regionExit] = newDestination;
            } while (newDestination == regionExit.SourceScene);
        }

        // Make sure that there is at least one exit that leads to the end level
        // The chosen region could be itself inaccessible but this still leads to less non-completable seeds
        if (_exitsOverrides.All(regionExit => regionExit.Value != 7))
        {
            var endExit = WorldGraph.Exits[random.Next(WorldGraph.Exits.Count)];

            _exitsOverrides[endExit] = 7;
        }

        // Generate start region
        _startRegion = _settings.StartLevel == SeedSettings.StartLevelSetting.Random
            ? random.Next(WorldGraph.LowestRegionNumber, WorldGraph.HighestRegionNumber)
            : (int)_settings.StartLevel + 1;
    }

    private void RandomFillItems(Random random, ItemPoolGenerator itemPoolGenerator)
    {
        // Randomize item locations content
        var availableItems = itemPoolGenerator.Retrieve();

        foreach (var itemLocation in _itemLocations)
        {
            var itemIndex = random.Next(availableItems.Count);

            itemLocation.ItemType = availableItems.RemoveAndReturn(itemIndex);
        }
    }

    private bool CheckCompletable()
    {
        // If trial keys are enabled, check that no key is located inside a trial
        if (_settings.TrialKeys)
        {
            foreach (var itemLocation in _itemLocations)
            {
                if (itemLocation is ChestItemLocation
                    {
                        ChestName: "Act04Room05To06_TreasureBox"
                                or "Act05_TreasureBox02_Room09To10"
                                or "Act03TreasureBox_Room05_02"
                    })
                {
                    if (itemLocation.ItemType == ItemSystem.ItemType.SPMaxAdd)
                    {
                        return false;
                    }
                }
            }
        }

        // Currently accessible regions, used to check exit requirements after each inventory update
        var accessibleRegions = new HashSet<Region>
        {
            WorldGraph.Regions[_startRegion]
        };

        // New regions that need exploration
        var regionsToExplore = new Queue<Region>();
        regionsToExplore.Enqueue(WorldGraph.Regions[_startRegion]);

        var inventory = new InventoryState(_settings);

        bool nonotaReachable = false;

        // One loop of the verification algorithm consist of:
        // - Explore a new region, take items while updating inventory, add exits as new regions (if not already visited)
        // - Recheck all transition requirements and add region if requirement is passed + not already visited
        while (regionsToExplore.Count > 0)
        {
            var region = regionsToExplore.Dequeue();

            // Update inventory
            if (region.ItemLocations is not null)
            {
                foreach (var itemLocation in region.ItemLocations)
                {
                    switch (itemLocation.ItemType)
                    {
                        case ItemSystem.ItemType.MagicNull:
                            inventory.ArcaneLevel++;
                            break;
                        case ItemSystem.ItemType.MagicIce:
                            inventory.IceLevel++;
                            break;
                        case ItemSystem.ItemType.MagicFire:
                            inventory.FireLevel++;
                            break;
                        case ItemSystem.ItemType.MagicLightning:
                            inventory.ThunderLevel++;
                            break;
                        case ItemSystem.ItemType.SPMaxAdd: // Token
                            inventory.TokenAmount++;
                            break;
                    }

                    // Used to count chest opened
                    if (itemLocation is ChestItemLocation)
                    {
                        inventory.ChestOpened++;
                    }
                }
            }

            // Update boss killed
            if (region.ContainsBoss)
            {
                inventory.BossKilled++;
            }

            // New regions
            var newRegions = new HashSet<Region>();

            // Add exits to new regions
            if (region.Exits is not null)
            {
                foreach (var regionExit in region.Exits)
                {
                    var destination = WorldGraph.Regions[_exitsOverrides[regionExit]];

                    // Mark that we reached nonota if it's the final region
                    if (destination.FinalRegion)
                    {
                        nonotaReachable = true;
                    }

                    newRegions.Add(destination);
                }
            }

            // Recheck transition requirements since inventory has been updated
            foreach (var accessibleRegion in accessibleRegions)
            {
                var transition = accessibleRegion.NextRegion;

                if (transition is not null && transition.Requirement.CheckRequirement(inventory))
                {
                    var destination = transition.Destination;

                    // End here if this is the end game region
                    if (destination.FinalRegion)
                    {
                        nonotaReachable = true;
                    }

                    newRegions.Add(destination);
                }
            }

            // Add new regions that were not already visited
            foreach (var newRegion in newRegions)
            {
                if (accessibleRegions.Add(newRegion))
                {
                    regionsToExplore.Enqueue(newRegion);
                }
            }
        }

        // Once everything has been explored, we check if nonota is reachable and the other end conditions
        bool endConditionsValidated = true;
        if (nonotaReachable)
        {
            // Magic master
            if (_settings.MagicMaster)
            {
                endConditionsValidated &= _settings.MagicUpgrade switch
                {
                    SeedSettings.MagicUpgradeMode.Vanilla => inventory is
                        {
                            ArcaneLevel: >= 5,
                            IceLevel: >= 5,
                            FireLevel: >= 5,
                            ThunderLevel: >= 5
                        },

                    SeedSettings.MagicUpgradeMode.BossKill => inventory is
                        {
                            ArcaneLevel: >= 1,
                            IceLevel: >= 1,
                            FireLevel: >= 1,
                            ThunderLevel: >= 1,
                            BossKilled: >= 4
                        },

                    _ => throw new ArgumentOutOfRangeException($"Invalid magic upgrade setting: {_settings.MagicUpgrade}")
                };
            }

            // Boss hunt
            if (_settings.BossHunt)
            {
                endConditionsValidated &= inventory.BossKilled >= NpcUtils.ValidBosses.Count;
            }

            // All chests opened
            if (_settings.AllChestOpened)
            {
                endConditionsValidated &= inventory.ChestOpened == WorldGraph.ChestsCount;
            }

            return endConditionsValidated;
        }

        return false;
    }
}