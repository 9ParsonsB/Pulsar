using System.Text.Json.Serialization;
using Observatory.Framework.Files.Converters;

namespace Observatory.Framework.Files.ParameterTypes;

public class Modifiers
{
    public string Label { get; init; }

    [JsonConverter(typeof(NumberOrStringConverter))]
    public NumberOrString Value { get; set; } 
    
    public double OriginalValue { get; init; }

    [JsonConverter(typeof(IntBoolConverter))]
    public bool LessIsGood { get; init; }
}

public class NumberOrString
{
    public NumberOrString()
    { }
    
    public NumberOrString(string value)
    {
        StringValue = value;
        IsString = true;
    }

    public NumberOrString(double value)
    {
        DoubleValue = value;
        IsDouble = true;
    }
    public string? StringValue { get; init; }
    public bool IsString { get; init; }
    public double? DoubleValue { get; init; }
    public bool IsDouble { get; init; }
}