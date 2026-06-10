namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when When another player joins your ship's crew.
/// </summary>
public class CrewMemberJoins : JournalBase
{
    public string Crew { get; init; }
    public bool Telepresence { get; init; }
}