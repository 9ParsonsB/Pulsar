namespace Observatory.Framework.Files.ParameterTypes;

using System.Text.Json.Serialization;

public class EngineerType
{
    public string Engineer { get; init; }
    public ulong EngineerID { get; init; }
    public int Rank { get; init; }
    public int RankProgress { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Progress Progress { get; init; }
}