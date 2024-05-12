using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Converters;

/// <summary>
/// The format used for materials changed from an object with a key for each material to an array of objects containing "name" and "percent".
/// Need to handle both if we're going to read historical data. This reads the old format into a class reflecting the new structure.
/// </summary>
public class MaterialCompositionConverter : JsonConverter<ImmutableList<MaterialComposition>>
{
    public override ImmutableList<MaterialComposition> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            var materialComposition = new List<MaterialComposition>();
            while (reader.Read())
            {
                if (reader.TokenType != JsonTokenType.EndObject)
                {
                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        var name = reader.GetString();
                        reader.Read();
                        var percent = reader.GetSingle();
                        var material = new MaterialComposition
                        {
                            Name = name,
                            Percent = percent
                        };
                        materialComposition.Add(material);
                    }
                }
                else
                {
                    break;
                }
            }
            return materialComposition.ToImmutableList();
        }

        return JsonSerializer.Deserialize<ImmutableList<MaterialComposition>>(ref reader, options)!;
    }

    public override void Write(Utf8JsonWriter writer, ImmutableList<MaterialComposition> value,
        JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}