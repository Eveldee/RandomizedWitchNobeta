using System.Linq;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;

namespace RandomizedWitchNobeta.Utils;

public static class UnityUtils
{
    public static GameObject FindGameObjectByNameForced(string name)
    {
        return Object.FindObjectsOfType<GameObject>(true)
            .FirstOrDefault(gameObject => gameObject.name == name);
    }

    public static Il2CppArrayBase<TComponent> FindComponentsByTypeForced<TComponent>() where TComponent : Component
    {
        return Object.FindObjectsOfType<TComponent>(true);
    }

    public static TComponent FindComponentByNameForced<TComponent>(string name)
    {
        var gameObject = FindGameObjectByNameForced(name);

        return gameObject is not null ? gameObject.GetComponent<TComponent>() : default;
    }
}