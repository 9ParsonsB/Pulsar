namespace Observatory.Framework.Files.Journal.StationServices;

/// <summary>
///     Written when refuelling (full tank).
/// </summary>
public class RefuelAll : JournalBase
{
    public int Cost { get; init; }
    public float Amount { get; init; }
}