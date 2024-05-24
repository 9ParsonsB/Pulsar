namespace Observatory.Framework.Files.Journal.StationServices;

[Obsolete("This event was removed in Elite Dangerous 3.0 and will only appear in legacy data.")]
public class EngineerApply : JournalBase
{
    public override string Event => "EngineerApply";
    public string Engineer { get; init; }
    public string Blueprint { get; init; }
    public int Level { get; init; }
}