using System;
using RandomizedWitchNobeta.Utils;
using RandomizedWitchNobeta.Utils.Nobeta;
using UnityEngine;

namespace RandomizedWitchNobeta.Features.Timer;

public class Timers : MonoBehaviour
{
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