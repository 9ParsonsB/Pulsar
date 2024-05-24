namespace Observatory.Framework.Files.Journal.StationServices;

public class CrewAssign : CrewFire
{
    public override string Event => "CrewAssign";
    public string Role { get; init; }
}