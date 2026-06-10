namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when When you force another player to leave your ship's crew.
/// </summary>
public class KickCrewMember : JournalBase
{
    public string Crew { get; init; }
    public bool OnCrime { get; init; }
    public bool Telepresence { get; init; }
}