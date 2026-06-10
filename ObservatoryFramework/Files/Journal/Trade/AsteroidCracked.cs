namespace Observatory.Framework.Files.Journal.Trade;

/// <summary>
///     Written when the player has broken up a 'Motherlode' asteroid for mining.
/// </summary>
public class AsteroidCracked : JournalBase
{
    public string Body { get; init; }
}