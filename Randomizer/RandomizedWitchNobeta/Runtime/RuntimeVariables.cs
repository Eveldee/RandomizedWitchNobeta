using System.Collections.Generic;
using System.Linq;
using RandomizedWitchNobeta.Generation.Models;
using RandomizedWitchNobeta.Utils;

namespace RandomizedWitchNobeta.Runtime;

public class RuntimeVariables
{
    public int StartScene { get; }

    public Dictionary<(int sourceScene, int nextSceneNumber, int nextSavePoint), (int sceneNumberOverride, int savePointOverride)>
        ExitsOverrides { get; } = new();

    public Dictionary<string, ItemSystem.ItemType> ChestOverrides { get; } = new();

    public RuntimeVariables(int startScene, Dictionary<RegionExit, int> exitsOverrides, List<ItemLocation> itemLocations)
    {
        StartScene = startScene;

        // Generate exit overrides
        foreach (var ((sourceScene, nextSceneNumber, nextSavePointNumber), destinationScene) in exitsOverrides)
        {
            ExitsOverrides[(sourceScene, nextSceneNumber, nextSavePointNumber)] =
                (destinationScene, SceneUtils.SceneStartSavePoint(destinationScene));
        }

        // Generate chest content overrides
        foreach (var chestItemLocation in itemLocations.OfType<ChestItemLocation>())
        {
            ChestOverrides[chestItemLocation.ChestName] = chestItemLocation.ItemType;
        }
    }
}