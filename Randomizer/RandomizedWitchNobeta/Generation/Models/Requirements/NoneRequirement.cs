namespace RandomizedWitchNobeta.Generation.Models.Requirements;

public class NoneRequirement : ITransitionRequirement
{
    public bool CheckRequirement(InventoryState inventoryState)
    {
        return true;
    }
}