using System;
using System.Collections.Generic;
using System.Linq;
using LibCpp2IL;
using RandomizedWitchNobeta.Generation.Models;
using RandomizedWitchNobeta.Runtime;
using RandomizedWitchNobeta.Utils;

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
        // Check Item Pool
        if (WorldGraph.ItemPool.Count != WorldGraph.ItemPoolSize)
        {
            Plugin.Log.LogError($"Invalid item pool size, expected '{WorldGraph.ItemPoolSize}' and found '{WorldGraph.ItemPool.Count}'. Aborting...");
            throw new Exception();
        }

        var random = new Random(_settings.Seed);

        Plugin.Log.LogMessage("Generating a seed...");
        int tries = 0;
        var startTime = DateTime.Now;

        do
        {
            tries++;

            GenerateExits(random);

            RandomFillItems(random);
        } while (!CheckCompletable());

        Plugin.Log.LogMessage($"A completable seed has been successfully generated in {tries} tries in {(DateTime.Now - startTime).TotalSeconds} seconds!");

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
                newDestination = random.Next(WorldGraph.LowestRegionNumber, WorldGraph.HighestRegionNumber + 1);

                _exitsOverrides[regionExit] = newDestination;
            } while (newDestination == regionExit.SourceScene);
        }

        // Make sure that there is a least one exit that leads to the end level
        // The chosen region could be itself inaccessible but this still leads to less non-completable seeds
        if (_exitsOverrides.All(regionExit => regionExit.Value != 7))
        {
            var endExit = WorldGraph.Exits[random.Next(WorldGraph.Exits.Count)];

            _exitsOverrides[endExit] = 7;
        }

        // Generate start region
        _startRegion = random.Next(WorldGraph.LowestRegionNumber, WorldGraph.HighestRegionNumber);
    }

    private void RandomFillItems(Random random)
    {
        // Randomize item locations content
        var availableItems = WorldGraph.ItemPool;

        foreach (var itemLocation in _itemLocations)
        {
            var itemIndex = random.Next(availableItems.Count);

            itemLocation.ItemType = availableItems.RemoveAndReturn(itemIndex);
        }
    }

    private bool CheckCompletable()
    {
        // Currently accessible regions, used to check exit requirements after each inventory update
        var accessibleRegions = new HashSet<Region>
        {
            WorldGraph.Regions[_startRegion]
        };

        // New regions that need exploration
        var regionsToExplore = new Queue<Region>();
        regionsToExplore.Enqueue(WorldGraph.Regions[_startRegion]);

        var inventory = new InventoryState();

        // One loop of the verification algorithm consist of:
        // - Explore a new region, take items while updating inventory, add exits as new regions (if not already visited)
        // - Recheck all transition requirements and add region if requirement is padded + not already visited
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
                }
            }

            // New regions
            var newRegions = new HashSet<Region>();

            // Add exits to new regions
            if (region.Exits is not null)
            {
                foreach (var regionExit in region.Exits)
                {
                    var destination = WorldGraph.Regions[_exitsOverrides[regionExit]];

                    // End here if this is the end game region
                    if (destination.FinalRegion)
                    {
                        return true;
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
                        return true;
                    }

                    newRegions.Add(destination);
                }
            }

            // Add new regions that were not already visited
            foreach (var newRegion in newRegions)
            {
                if (!accessibleRegions.Contains(newRegion))
                {
                    accessibleRegions.Add(newRegion);
                    regionsToExplore.Enqueue(newRegion);
                }
            }
        }

        return false;
    }
}