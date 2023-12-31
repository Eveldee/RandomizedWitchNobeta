﻿using System;
using System.Diagnostics;
using RandomizedWitchNobeta.Config;
using RandomizedWitchNobeta.Utils;
using UnityEngine;

namespace RandomizedWitchNobeta.Timer;

[Section("Timers")]
public class Timers : MonoBehaviour
{
    [Bind]
    public static bool ShowTimers = false;
    [Bind]
    public static bool ShowRealTime = true;
    [Bind]
    public static bool ShowLoadRemovedTimer = true;

    private bool _paused = true;
    private bool _ended = false;

    public void Update()
    {
        if (!_ended && !_paused && Singletons.RuntimeVariables is { } runtimeVariables)
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

    public void End()
    {
        _ended = true;
    }

    public void Reset()
    {
        _ended = false;
    }
}