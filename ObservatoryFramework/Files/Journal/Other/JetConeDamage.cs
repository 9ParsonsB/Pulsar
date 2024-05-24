namespace Observatory.Framework.Files.Journal.Other;

public class JetConeDamage : JournalBase
{
    public override string Event => "JetConeDamage";
    public string Module { get; init; }
}