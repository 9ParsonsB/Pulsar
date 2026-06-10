namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when this player has joined a wing.
/// </summary>
public class WingJoin : JournalBase
{
    public IList<string> Others { get; init; }
}