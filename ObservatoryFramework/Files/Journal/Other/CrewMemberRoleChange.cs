namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when in Multicrew, Helm's log, when another crew player changes role.
/// </summary>
public class CrewMemberRoleChange : CrewMemberJoins
{
    public string Role { get; init; }
}