namespace RandomizedWitchNobeta.Generation.Models;

public class Region
{
    public required string FriendlyName { get; init; }

    public required RegionExit[] Exits { get; init; }

    public required ItemLocation[] ItemLocations { get; init; }

    public required RegionTransition NextRegion { get; init; }

    public bool FinalRegion { get; init; } = false;
}