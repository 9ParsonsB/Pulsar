namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when When you join another player ship's crew.
/// </summary>
public class JoinACrew : JournalBase
{
    public string Captain { get; init; }
    public bool Telepresence { get; init; }
}