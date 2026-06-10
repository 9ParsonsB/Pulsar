namespace Observatory.Framework.Files.Journal.Odyssey;

/// <summary>
///     Written when an Apex taxi booking is cancelled.
/// </summary>
public class CancelTaxi : JournalBase
{
    public int Refund { get; init; }
}