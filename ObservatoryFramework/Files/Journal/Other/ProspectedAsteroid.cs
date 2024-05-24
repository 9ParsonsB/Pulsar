using System.Collections.Immutable;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Other;

public class ProspectedAsteroid : JournalBase
{
    public override string Event => "ProspectedAsteroid";
    public ImmutableList<ProspectMaterial> Materials { get; init; }
    public string Content { get; init; }
    public string Content_Localised { get; init; }
    public string MotherlodeMaterial { get; init; }
    public string MotherlodeMaterial_Localised { get; init; }
    public float Remaining { get; init; }
}