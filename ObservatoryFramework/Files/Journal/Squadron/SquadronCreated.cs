namespace Observatory.Framework.Files.Journal.Squadron;

/// <summary>
///     Written when a squadron is created.
/// </summary>
public class SquadronCreated : JournalBase
{
    public string SquadronName { get; init; }
}