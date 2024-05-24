namespace Observatory.Framework.Files.Journal.Powerplay;

public class PowerplayFastTrack : PowerplayJoin
{
    public override string Event => "PowerplayFastTrack";
    public int Cost { get; init; }
}