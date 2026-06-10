namespace Observatory.Framework.Files.ParameterTypes;

using System.Text.Json.Serialization;

public class MicroTransfer : MicroResource
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MicroTransferDirection Direction { get; init; }
}