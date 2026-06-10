namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when scooping fuel from a star.
/// </summary>
public class FuelScoop : JournalBase
{
    public float Scooped { get; init; }
    public float Total { get; init; }
}