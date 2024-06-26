﻿using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.Odyssey;

public class BuyMicroResources : JournalBase
{
    public override string Event => "BuyMicroResources";
    public string Name { get; init; }
    public string Name_Localised { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MicroCategory Category { get; init; }
    public int Count { get; init; }
    public int Price { get; init; }
    public ulong MarketID { get; init; }
    public int TotalCount { get; init; }
    public List<MicroResource> MicroResources { get; init; }
}