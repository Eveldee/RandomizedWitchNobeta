using System.Collections.Generic;

namespace RandomizedWitchNobeta.Utils.Nobeta;

public static class EnvironmentUtils
{
    public static ISet<string> LavaTrapNamePrefix { get; } = new HashSet<string>
    {
        "_Lava",
        "ChageFogLava",
        "ChangeFogLava",
        "Tarp_Lava",
        "Trap_Lava"
    };
}