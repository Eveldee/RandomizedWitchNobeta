using RandomizedWitchNobeta.Generation.Models.Requirements;

namespace RandomizedWitchNobeta.Generation.Models;

public class RegionTransition
{
    public required Region Destination { get; init; }
    public required ITransitionRequirement Requirement { get; init; }
}