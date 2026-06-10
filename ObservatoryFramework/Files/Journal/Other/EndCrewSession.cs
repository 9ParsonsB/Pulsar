namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when the captain in multicrew disbands the crew.
/// </summary>
public class EndCrewSession : JournalBase
{
    public bool OnCrime { get; init; }
    public bool Telepresence { get; init; }
}