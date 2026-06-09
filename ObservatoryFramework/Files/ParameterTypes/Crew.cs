using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Observatory.Framework.Files.ParameterTypes;

[Owned]
public class Crew
{
    [JsonPropertyName("NpcCrew_TotalWages")]
    public long NpcCrewTotalWages { get; init; }

    [JsonPropertyName("NpcCrew_Hired")]
    public int NpcCrewHired { get; init; }

    [JsonPropertyName("NpcCrew_Fired")]
    public int NpcCrewFired { get; init; }

    [JsonPropertyName("NpcCrew_Died")]
    public int NpcCrewDied { get; init; }
}