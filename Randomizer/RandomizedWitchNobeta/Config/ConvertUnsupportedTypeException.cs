using System;

namespace RandomizedWitchNobeta.Config;

public class ConvertUnsupportedTypeException : AutoConfigException
{
    public ConvertUnsupportedTypeException(IBindConverter converter, Type targetType) : base($"Converter {converter.GetType().Name} does not support type {targetType}")
    {

    }
}