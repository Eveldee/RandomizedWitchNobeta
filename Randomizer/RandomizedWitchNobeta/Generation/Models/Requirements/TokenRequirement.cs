namespace RandomizedWitchNobeta.Generation.Models.Requirements;

public class TokenRequirement : ITransitionRequirement
{
    public int AmountNeeded { get; }

    public TokenRequirement(int amountNeeded)
    {
        AmountNeeded = amountNeeded;
    }

    public bool CheckRequirement(InventoryState inventoryState)
    {
        return inventoryState.TokenAmount >= AmountNeeded;
    }
}