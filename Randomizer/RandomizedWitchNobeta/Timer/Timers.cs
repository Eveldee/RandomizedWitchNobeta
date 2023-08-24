using System;
using System.Diagnostics;
using RandomizedWitchNobeta.Config;
using RandomizedWitchNobeta.Utils;
using UnityEngine;

namespace RandomizedWitchNobeta.Timer;

[Section("Timers")]
public class Timers : MonoBehaviour
{
    [Bind]
    public static bool ShowTimers;
    [Bind]
    public static bool ShowRealTime;
    [Bind]
    public static bool ShowLoadRemovedTimer;

    private bool _paused = true;

    public void Update()
    {
        if (!_paused && Singletons.RuntimeVariables is { } runtimeVariables)
        {
            var elapsed = TimeSpan.FromSeconds(Time.deltaTime);

            runtimeVariables.ElapsedRealTime += elapsed;

            if (!SceneUtils.IsLoading)
            {
                runtimeVariables.ElapsedLoadRemoved += elapsed;
            }
        }
    }

    public void Pause()
    {
        _paused = true;
    }

    public void Resume()
    {
        _paused = false;
    }
}