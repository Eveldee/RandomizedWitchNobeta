namespace RandomizedWitchNobeta.Generation.Models;

public class ChestItemLocation : ItemLocation
{
    public string ChestName { get; }

    public ChestItemLocation(string chestName)
    {
        ChestName = chestName;
    }
}