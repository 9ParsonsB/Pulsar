using System.Collections.Immutable;

namespace Observatory.Framework.Files.Journal.StationServices;

public class Repair : JournalBase
{
    public override string Event => "Repair";
    public string Item { get; init; }
    public int Cost { get; init; }
    public ICollection<string> Items { get; init; }
}