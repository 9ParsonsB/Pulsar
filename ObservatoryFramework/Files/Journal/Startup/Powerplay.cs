namespace Observatory.Framework.Files.Journal.Startup;

/// <summary>
///     Written at startup, if player has pledged to a power.
/// </summary>
public class Powerplay : JournalBase
{
    public string Power { get; init; }

    public int Rank { get; init; }

    public int Merits { get; init; }

    public int Votes { get; init; }

    public long TimePledged { get; init; }
}