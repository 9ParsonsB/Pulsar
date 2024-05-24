using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestProject1;

public class Tests
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "event")]
    [JsonDerivedType(typeof(ChildEvent), "child")]
    public abstract class EventBase
    {
        [JsonPropertyName("timestamp")] public DateTimeOffset Timestamp { get; init; }

        [JsonPropertyName("event")] public string Event { get; set; }
    }

    public class ChildEvent : EventBase;

    [Test]
    public void Test()
    {
        var json = """
                   { "event": "child", "timestamp":"2024-05-20T12:36:10Z" }
                   """;

        var obj = JsonSerializer.Deserialize<EventBase>(json)!;

        Console.WriteLine(obj.Event); // ""
    }
}