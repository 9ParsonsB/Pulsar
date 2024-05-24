namespace Observatory.Framework.Files.Journal.StationServices;

public class EngineerLegacyConvert : EngineerCraft
{
    public override string Event => "EngineerLegacyConvert";
    public bool IsPreview { get; init; }
}