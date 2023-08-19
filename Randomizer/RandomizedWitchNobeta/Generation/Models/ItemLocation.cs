namespace RandomizedWitchNobeta.Generation.Models;

public abstract class ItemLocation
{
    public ItemSystem.ItemType ItemType { get; set; } = ItemSystem.ItemType.Null;
}