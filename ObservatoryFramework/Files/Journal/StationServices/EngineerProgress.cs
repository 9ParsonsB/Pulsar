using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.StationServices;

public class EngineerProgress : JournalBase
{
    public override string Event => "EngineerProgress";
    public string Engineer { get; init; }
    public ulong EngineerID { get; init; }
    public int Rank { get; init; }
    public int RankProgress { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Progress Progress { get; init; }

    public ImmutableList<EngineerType> Engineers { get; init; }
}