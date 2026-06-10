namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when the player restarts after death.
/// </summary>
public class Resurrect : JournalBase
{
    public string Option { get; init; }
    public int Cost { get; init; }
    public bool Bankrupt { get; init; }
}