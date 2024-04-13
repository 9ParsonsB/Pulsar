﻿using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.FleetCarrier;

public class CarrierShipPack : JournalBase
{
    public ulong CarrierID { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CarrierOperation Operation { get; init; }
    public string PackTheme { get; init; }
    public int PackTier { get; init; }
    public int Cost { get; init; }
    public int Refund { get; init; }
}