using System;

namespace RandomizedWitchNobeta.Config;

[AttributeUsage(AttributeTargets.Class)]
public class SectionAttribute : Attribute
{
    public string SectionName { get; }

    public SectionAttribute(string sectionName)
    {
        SectionName = sectionName;
    }
}