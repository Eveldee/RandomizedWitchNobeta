namespace RandomizedWitchNobeta.Generation.Models.Requirements;
public class MagicRequirement : ITransitionRequirement
{

    public MagicType Type { get; }
    public int Level { get; }

    public MagicRequirement(MagicType type, int level)
    {
        Type = type;
        Level = level;
    }

    public bool CheckRequirement(InventoryState inventoryState)
    {
        return Type switch
        {
            MagicType.Arcane => inventoryState.ArcaneLevel >= Level,
            MagicType.Ice => inventoryState.IceLevel >= Level,
            MagicType.Fire => inventoryState.FireLevel >= Level,
            MagicType.Thunder => inventoryState.ThunderLevel >= Level,
            _ => false
        };
    }
}

public enum MagicType
{
    Arcane,
    Ice,
    Fire,
    Thunder
}