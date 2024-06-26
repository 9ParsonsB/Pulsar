﻿namespace Observatory.Framework.Files.Journal.StationServices;

public class ModuleSellRemote : JournalBase
{
    public override string Event => "ModuleSellRemote";
    public int StorageSlot { get; init; }
    public string SellItem { get; init; }
    public string SellItem_Localised { get; init; }
    public ulong ServerId { get; init; }
    public uint SellPrice { get; init; }
    public string Ship { get; init; }
    public ulong ShipID { get; init; }
}