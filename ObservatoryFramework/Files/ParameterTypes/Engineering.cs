namespace Observatory.Framework.Files.ParameterTypes;

using Microsoft.EntityFrameworkCore;

[Owned]
public class Engineering
{
    public ulong EngineerID { get; init; }

    public string? Engineer { get; init; }

    public ulong BlueprintID { get; init; }

    public string BlueprintName { get; init; }

    public int Level { get; init; }

    public double Quality { get; init; }

    public string? ExperimentalEffect { get; init; }

    public string? ExperimentalEffect_Localised { get; init; }

    public List<Modifiers>? Modifiers { get; init; }
}