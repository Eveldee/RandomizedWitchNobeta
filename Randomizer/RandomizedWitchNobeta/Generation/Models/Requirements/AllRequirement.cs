using System.Collections.Generic;
using System.Linq;

namespace RandomizedWitchNobeta.Generation.Models.Requirements;

public class AllRequirement : MultiRequirement
{
    public AllRequirement(List<ITransitionRequirement> requirements) : base(requirements)
    {

    }

    public override bool CheckRequirement(InventoryState inventoryState)
    {
        return _requirements.All(requirement => requirement.CheckRequirement(inventoryState));
    }
}