namespace RandomizedWitchNobeta.Generation.Models.Requirements;

public interface ITransitionRequirement
{
    public bool CheckRequirement(InventoryState inventoryState);
}