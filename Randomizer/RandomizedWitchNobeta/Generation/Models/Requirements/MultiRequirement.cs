using System.Collections.Generic;

namespace RandomizedWitchNobeta.Generation.Models.Requirements;

public abstract class MultiRequirement : ITransitionRequirement
{
    protected readonly List<ITransitionRequirement> _requirements;

    protected MultiRequirement(List<ITransitionRequirement> requirements)
    {
        _requirements = requirements;
    }

    public abstract bool CheckRequirement(InventoryState inventoryState);
}