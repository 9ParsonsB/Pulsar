namespace Observatory.Framework.Files.Journal.Other;

public class CrewMemberRoleChange : CrewMemberJoins
{
    public override string Event => "CrewMemberRoleChange";
    public string Role { get; init; }
}