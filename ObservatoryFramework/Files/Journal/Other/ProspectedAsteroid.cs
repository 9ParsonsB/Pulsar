namespace Observatory.Framework.Files.Journal.Other;

using ParameterTypes;

/// <summary>
///     Written when an asteroid is prospected.
/// </summary>
public class ProspectedAsteroid : JournalBase
{
    public List<ProspectMaterial> Materials { get; init; }
    public string Content { get; init; }
    public string Content_Localised { get; init; }
    public string MotherlodeMaterial { get; init; }
    public string MotherlodeMaterial_Localised { get; init; }
    public float Remaining { get; init; }
}