﻿using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.StationServices;

public class EngineerContribution : JournalBase
{
    public string Engineer { get; init; }
    public ulong EngineerID { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ContributionType Type { get; init; }
    public string Commodity { get; init; }
    public string Commodity_Localised { get; init; }
    public string Material { get; init; }
    public string Material_Localised { get; init; }
    public string Faction { get; init; }
    public int Quantity { get; init; }
    public int TotalQuantity { get; init; }
}