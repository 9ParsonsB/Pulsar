namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when purchasing ammunition.
/// </summary>
public class BuyAmmo : JournalBase
{
    public int Cost { get; init; }
}