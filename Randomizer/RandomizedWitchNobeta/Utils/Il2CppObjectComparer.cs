using System.Collections.Generic;
using Il2CppSystem;

namespace RandomizedWitchNobeta.Utils;

public class Il2CppObjectComparer : IEqualityComparer<Object>
{
    public bool Equals(Object x, Object y)
    {
        return Object.ReferenceEquals(x, y);
    }

    public int GetHashCode(Object obj)
    {
        return Object.InternalGetHashCode(obj);
    }
}