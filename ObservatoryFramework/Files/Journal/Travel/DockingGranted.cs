namespace Observatory.Framework.Files.Journal.Travel;

public class DockingGranted : DockingCancelled
{
    public override string Event => "DockingGranted";
    public int LandingPad { get; init; }
}