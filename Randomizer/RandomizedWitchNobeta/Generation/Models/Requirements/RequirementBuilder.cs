using System;
using System.Collections.Generic;

namespace RandomizedWitchNobeta.Generation.Models.Requirements;

public enum CombineMethod
{
    All,
    OneOf
}

public class RequirementBuilder
{
    private readonly List<ITransitionRequirement> _requirements;
    private readonly CombineMethod _combineMethod;

    public RequirementBuilder(CombineMethod combineMethod = CombineMethod.OneOf)
    {
        _combineMethod = combineMethod;
        _requirements = new List<ITransitionRequirement>();
    }

    public ITransitionRequirement Build()
    {
        if (_requirements.Count == 0)
        {
            return new NoneRequirement();
        }

        if (_requirements.Count == 1)
        {
            return _requirements[0];
        }

        return _combineMethod switch
        {
            CombineMethod.All => new AllRequirement(_requirements),
            CombineMethod.OneOf => new OneOfRequirement(_requirements),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public RequirementBuilder Magic(MagicType type, int level)
    {
        _requirements.Add(new MagicRequirement(type, level));

        return this;
    }

    public RequirementBuilder Token(int amount)
    {
        _requirements.Add(new TokenRequirement(amount));

        return this;
    }

    public RequirementBuilder All(Action<RequirementBuilder> action)
    {
        var builder = new RequirementBuilder(CombineMethod.All);

        action(builder);

        _requirements.Add(builder.Build());

        return this;
    }

    public RequirementBuilder OneOf(Action<RequirementBuilder> action)
    {
        var builder = new RequirementBuilder(CombineMethod.OneOf);

        action(builder);

        _requirements.Add(builder.Build());

        return this;
    }
}