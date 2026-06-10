namespace Observatory.Framework.Files.Journal.Other;

using Converters;
using ParameterTypes;
using System.Text.Json.Serialization;

/// <summary>
///     Written when synthesis is used to repair or rearm.
/// </summary>
public class Synthesis : JournalBase
{
    public string Name { get; init; }

    [JsonConverter(typeof(MaterialConverter))]
    public List<Material> Materials { get; init; }
}