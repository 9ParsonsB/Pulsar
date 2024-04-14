using Observatory.Framework.Files.Journal;
using Observatory.Framework.Files.Journal.Exploration;

namespace Pulsar.Utils;

public class JournalReader
{
    public static TJournal ObservatoryDeserializer<TJournal>(string json) where TJournal : JournalBase
    {
        TJournal deserialized;

        if (typeof(TJournal) == typeof(InvalidJson))
        {
            InvalidJson invalidJson;
            try
            {
                var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
                var eventType = string.Empty;
                var timestamp = string.Empty;

                while ((eventType == string.Empty || timestamp == string.Empty) && reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        if (reader.GetString() == "event")
                        {
                            reader.Read();
                            eventType = reader.GetString();
                        }
                        else if (reader.GetString() == "timestamp")
                        {
                            reader.Read();
                            timestamp = reader.GetString();
                        }
                    }
                }

                invalidJson = new InvalidJson
                {
                    Event = "InvalidJson",
                    Timestamp = DateTimeOffset.UnixEpoch,
                    OriginalEvent = eventType
                };
            }
            catch
            {
                invalidJson = new InvalidJson
                {
                    Event = "InvalidJson",
                    Timestamp = DateTimeOffset.UnixEpoch,
                    OriginalEvent = "Invalid"
                };
            }

            deserialized = (TJournal)Convert.ChangeType(invalidJson, typeof(TJournal));
        }
        //Journal potentially had invalid JSON for a brief period in 2017, check for it and remove.
        //TODO: Check if this gets handled by InvalidJson now.
        else if (typeof(TJournal) == typeof(Scan) && json.Contains("\"RotationPeriod\":inf"))
        {
            deserialized = JsonSerializer.Deserialize<TJournal>(json.Replace("\"RotationPeriod\":inf,", ""));
        }
        else
        {
            deserialized = JsonSerializer.Deserialize<TJournal>(json);
        }

        return deserialized;
    }
}