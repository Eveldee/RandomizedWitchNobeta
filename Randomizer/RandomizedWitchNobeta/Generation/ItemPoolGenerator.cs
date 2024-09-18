using System;
using System.Collections.Generic;
using System.Linq;
using RandomizedWitchNobeta.Shared;
using Weighted_Randomizer;

namespace RandomizedWitchNobeta.Generation;

public class ItemPoolGenerator
{
    public const int ItemPoolSize = 45 + 1; // 45 Chest + 1 Cat check

    private readonly List<ItemSystem.ItemType> _pool = new(ItemPoolSize);

    public ItemPoolGenerator(SeedSettings settings, Random random)
    {
        FillItemPool(settings, random);
    }

    public List<ItemSystem.ItemType> Retrieve() => _pool.ToList();

    private void FillItemPool(SeedSettings settings, Random random)
    {
        // Magic books
        _pool.Add(ItemSystem.ItemType.MagicIce);
        _pool.Add(ItemSystem.ItemType.MagicFire);
        _pool.Add(ItemSystem.ItemType.MagicLightning);

        if (settings.NoArcane)
        {
            _pool.Add(ItemSystem.ItemType.MagicNull);
        }

        if (settings.MagicUpgrade == SeedSettings.MagicUpgradeMode.Vanilla)
        {
            _pool.AddRange(Enumerable.Repeat(ItemSystem.ItemType.MagicNull, 4));
            _pool.AddRange(Enumerable.Repeat(ItemSystem.ItemType.MagicIce, 4));
            _pool.AddRange(Enumerable.Repeat(ItemSystem.ItemType.MagicFire, 4));
            _pool.AddRange(Enumerable.Repeat(ItemSystem.ItemType.MagicLightning, 4));
            _pool.AddRange(Enumerable.Repeat(ItemSystem.ItemType.Absorb, 4));
            _pool.AddRange(Enumerable.Repeat(ItemSystem.ItemType.SkyJump, 4));
        }

        if (settings.MagicUpgrade == SeedSettings.MagicUpgradeMode.BossKill && settings.BookAmount > 1)
        {
            var toAdd = settings.BookAmount - 1;

            _pool.AddRange(Enumerable.Repeat(ItemSystem.ItemType.MagicNull, toAdd));
            _pool.AddRange(Enumerable.Repeat(ItemSystem.ItemType.MagicIce, toAdd));
            _pool.AddRange(Enumerable.Repeat(ItemSystem.ItemType.MagicFire, toAdd));
            _pool.AddRange(Enumerable.Repeat(ItemSystem.ItemType.MagicLightning, toAdd));
            _pool.AddRange(Enumerable.Repeat(ItemSystem.ItemType.Absorb, toAdd));
            _pool.AddRange(Enumerable.Repeat(ItemSystem.ItemType.SkyJump, toAdd));
        }

        // Trial keys
        if (settings.TrialKeys)
        {
            _pool.AddRange(Enumerable.Repeat(ItemSystem.ItemType.SPMaxAdd, settings.TrialKeysAmount));
        }

        // Bag increase
        _pool.AddRange(Enumerable.Repeat(ItemSystem.ItemType.BagMaxAdd, 4));

        // Fill
        var filler = new DynamicWeightedRandomizer<int>(random.Next())
        {
            { (int)ItemSystem.ItemType.Null, settings.ItemWeightSouls },
            { (int)ItemSystem.ItemType.HPCureBig, settings.ItemWeightHP },
            { (int)ItemSystem.ItemType.MPCureBig, settings.ItemWeightMP },
            { (int)ItemSystem.ItemType.DefenseB, settings.ItemWeightDefense },
            { (int)ItemSystem.ItemType.HolyB, settings.ItemWeightHoly },
            { (int)ItemSystem.ItemType.MysteriousB, settings.ItemWeightArcane }
        };

        while (_pool.Count < ItemPoolSize)
        {
            _pool.Add((ItemSystem.ItemType) filler.NextWithReplacement());
        }

        // Check size
        if (_pool.Count != ItemPoolSize)
        {
            Plugin.Log.LogError($"Invalid item pool size, expected '{ItemPoolSize}' and found '{_pool.Count}'. Aborting...");
            throw new Exception();
        }
    }
}