using System;

namespace RandomizedWitchNobeta.Config;

public abstract class AutoConfigException : Exception
{
    protected AutoConfigException(string message) : base(message)
    {

    }
}