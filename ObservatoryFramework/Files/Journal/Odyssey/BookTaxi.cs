namespace Observatory.Framework.Files.Journal.Odyssey;

/// <summary>
///     Written when booking a taxi transport.
/// </summary>
public class BookTaxi : JournalBase
{
    public int Cost { get; init; }
    public string DestinationSystem { get; init; }
    public string DestinationLocation { get; init; }
    public bool Retreat { get; init; }
}