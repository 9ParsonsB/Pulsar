namespace Observatory.Framework.Files.Journal.Startup;

public class NewCommander : Commander
{
    public override string Event => "NewCommander";
    public string Package { get; init; }
}