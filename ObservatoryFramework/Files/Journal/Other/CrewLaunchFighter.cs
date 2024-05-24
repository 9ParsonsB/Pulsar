namespace Observatory.Framework.Files.Journal.Other;

public class CrewLaunchFighter : CrewMemberJoins
{
    public override string Event => "CrewLaunchFighter";
    public ulong ID { get; init; }
}