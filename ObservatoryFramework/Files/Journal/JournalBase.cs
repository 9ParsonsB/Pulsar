using System.Globalization;
using System.Text.Json.Serialization;

namespace Observatory.Framework.Files.Journal;

public class JournalBase
{
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; init; }

    [JsonIgnore]
    public DateTimeOffset TimestampDateTime
    {
        get => ParseDateTime(Timestamp);
    }

    [JsonPropertyName("event")]
    public string Event { get;  init; }

    [JsonExtensionData]
    public Dictionary<string, object> AdditionalProperties { get; init; }

    [JsonIgnore]
    public string Json 
    {
        get => json; 
        set
        {
                if (json == null || string.IsNullOrWhiteSpace(json))
                {
                    json = value;
                }
                else
                {
                    throw new Exception("Journal property \"Json\" can only be set while empty.");
                }
            }
    }

    private string json;

    // For use by Journal object classes for .*DateTime properties, like TimestampeDateTime, above.
    internal static DateTimeOffset ParseDateTime(string value)
    {
            if (DateTime.TryParseExact(value, "yyyy-MM-ddTHH:mm:ssZ", null, DateTimeStyles.AssumeUniversal, out var dateTimeValue))
            {
                return dateTimeValue;
            }

            return new DateTime();
        }
}