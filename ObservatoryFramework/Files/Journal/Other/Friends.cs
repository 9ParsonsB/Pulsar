using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Other;

public class Friends : JournalBase
{
    public override string Event => "Friends";
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public FriendStatus Status { get; init; }
    public string Name { get; init; }
}