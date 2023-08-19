using System.Collections.Generic;
using System.Linq;

namespace RandomizedWitchNobeta.Generation.Models.Requirements;

public class OneOfRequirement : MultiRequirement
{
    public OneOfRequirement(List<ITransitionRequirement> requirements) : base(requirements)
    {

    }

    public override bool CheckRequirement(InventoryState inventoryState)
    {
        return _requirements.Any(requirement => requirement.CheckRequirement(inventoryState));
    }
}