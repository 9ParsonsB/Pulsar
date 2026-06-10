namespace Observatory.Framework.Files.Journal.Powerplay;

/// <summary>
///     Written when paying to fast-track allocation of commodities.
/// </summary>
public class PowerplayFastTrack : PowerplayJoin
{
    public int Cost { get; init; }
}