using Microsoft.EntityFrameworkCore;

namespace Observatory.Framework.Files.ParameterTypes;

[Owned]
public class Mission
{
    public ulong MissionID { get; init; }

    public string Name { get; init; }

    public bool PassengerMission { get; init; }

    public int Expires { get; init; }
}