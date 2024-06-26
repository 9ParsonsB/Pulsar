﻿namespace Observatory.Framework.Files.Journal.StationServices;

public class ModuleSell : JournalBase
{
    public override string Event => "ModuleSell";
    public ulong MarketID { get; init; }
    public string Slot { get; init; }
    public string SellItem { get; init; }
    public string SellItem_Localised { get; init; }
    public uint SellPrice { get; init; }
    public string Ship { get; init; }
    public ulong ShipID { get; init; }
}