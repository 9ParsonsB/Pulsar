namespace Observatory.Framework.Files.Journal.Startup;

public class Commander : JournalBase
{
    public override string Event => "Commander";
    public string Name { get; init; }

    public string FID { get; init; }
}