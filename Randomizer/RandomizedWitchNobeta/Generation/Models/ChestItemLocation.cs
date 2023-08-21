namespace RandomizedWitchNobeta.Generation.Models;

public class ChestItemLocation : ItemLocation
{
    public string ChestName { get; }
    public int SceneNumber { get; }

    public ChestItemLocation(string chestName, int sceneNumber)
    {
        ChestName = chestName;
        SceneNumber = sceneNumber;
    }
}