namespace Observatory.Framework.Files.Journal.Other;

public class JetConeBoost : JournalBase
{
    public override string Event => "JetConeBoost";
    public float BoostValue { get; init; }
}