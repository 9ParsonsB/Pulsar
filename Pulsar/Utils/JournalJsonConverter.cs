using Observatory.Framework.Files.Journal.Combat;
using Observatory.Framework.Files.Journal.Exploration;
using Observatory.Framework.Files.Journal.Odyssey;
using Observatory.Framework.Files.Journal.Other;
using Observatory.Framework.Files.Journal.Powerplay;
using Observatory.Framework.Files.Journal.Startup;
using Observatory.Framework.Files.Journal.StationServices;
using Observatory.Framework.Files.Journal.Trade;
using Observatory.Framework.Files.Journal.Travel;

namespace Pulsar.Utils;

using Observatory.Framework.Files.Journal;

[Flags]
public enum JournalReaderState
{
    /// <summary>
    /// Have read the first character of the object
    /// </summary>
    Start,

    /// <summary>
    /// Have read the timestamp. Generally the first property in a journal entry.
    /// </summary>
    Timestamp,

    /// <summary>
    /// have read the event name. Generally the second property in a journal entry.
    /// </summary>
    Event,

    /// <summary>
    /// Have read the last character of the object, the next character should be a newline, whitespace, EOF, or another object.
    /// </summary>
    End,
}

/// <summary>
/// A JournalFile contains a collection of journal entries.
/// Each journal entry is a JSON object, delimited by a newline character.
/// all Journals can be deserialized into a JournalBase object for identification
/// and then deserialized into their respective types.
/// </summary>
public class JournalJsonConverter(ILogger<JournalJsonConverter> logger) : JsonConverter<JournalBase>
{
    private JournalReaderState state = JournalReaderState.Start;

    public override JournalBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Utf8JsonReader clone = reader;
        DateTimeOffset? timestamp = null;
        string? eventName = null;
        // for debug
        int depth = 0;
        do
        {
            depth++;
            switch (clone.TokenType)
            {
                case JsonTokenType.StartObject:
                    state = JournalReaderState.Start;
                    break;
                case JsonTokenType.EndObject:
                    state = JournalReaderState.End;
                    break;
                case JsonTokenType.PropertyName:
                    var propertyName = clone.GetString();
                    // if we have not started reading the body, and we have not read the (timestamp or event name) 
                    if ((state & JournalReaderState.Timestamp) == 0 || (state & JournalReaderState.Event) == 0)
                    {
                        switch (propertyName)
                        {
                            case "timestamp":
                                clone.Read();
                                timestamp = clone.GetDateTimeOffset();
                                state = JournalReaderState.Timestamp;
                                break;
                            case "event":
                                clone.Read();
                                eventName = clone.GetString();
                                state = JournalReaderState.Event;
                                break;
                        }
                    }

                    if ((state & JournalReaderState.Event) != 0)
                    {
                        // create destination type
                        return GetDestinationType(ref reader, eventName!);
                    }

                    break;
                case JsonTokenType.Comment:
                    continue;
                case JsonTokenType.None:
                case JsonTokenType.StartArray:
                case JsonTokenType.EndArray:
                case JsonTokenType.String:
                case JsonTokenType.Number:
                case JsonTokenType.True:
                case JsonTokenType.False:
                case JsonTokenType.Null:
                    logger.LogWarning("Unexpected token type {TokenType} at depth {Depth}", clone.TokenType, depth);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        } while (clone.Read());

        logger.LogWarning("Failed to deserialize journal entry at depth: {Depth}. Event?: {EventName}, Timestamp?: {Timestamp}", depth, eventName, timestamp);
        return null;
        // TODO: handle inf (invalid data) in the journal files
        // else if (typeof(TJournal) == typeof(Scan) && json.Contains("\"RotationPeriod\":inf"))
        // {
        //     deserialized = JsonSerializer.Deserialize<TJournal>(json.Replace("\"RotationPeriod\":inf,", ""));
        // }
    }

    private JournalBase GetDestinationType(ref Utf8JsonReader reader, string eventName)
    {
        switch (eventName)
        {
            case "Fileheader":
                return JsonSerializer.Deserialize<FileHeader>(ref reader)!;
            case "Commander":
                return JsonSerializer.Deserialize<Commander>(ref reader)!;
            case "Materials":
                return JsonSerializer.Deserialize<Materials>(ref reader)!;
            case "Rank":
                return JsonSerializer.Deserialize<Rank>(ref reader)!;
            case "Music":
                return JsonSerializer.Deserialize<Music>(ref reader)!;
            case "Cargo":
                return JsonSerializer.Deserialize<Cargo>(ref reader)!;
            case "Loadout":
                return JsonSerializer.Deserialize<Loadout>(ref reader)!;
            case "Missions":
                return JsonSerializer.Deserialize<Missions>(ref reader)!;
            case "FSSSignalDiscovered":
                return JsonSerializer.Deserialize<FSSSignalDiscovered>(ref reader)!;
            case "Reputation":
                return JsonSerializer.Deserialize<Reputation>(ref reader)!;
            case "LoadGame":
                return JsonSerializer.Deserialize<LoadGame>(ref reader)!;
            case "ReceiveText":
                return JsonSerializer.Deserialize<ReceiveText>(ref reader)!;
            case "ShipLocker":
                return JsonSerializer.Deserialize<ShipLockerMaterials>(ref reader)!;
            case "Location":
                return JsonSerializer.Deserialize<Location>(ref reader)!;
            case "Powerplay":
                return JsonSerializer.Deserialize<Powerplay>(ref reader)!;
            case "ReservoirReplenished":
                return JsonSerializer.Deserialize<ReservoirReplenished>(ref reader)!;
            case "Statistics":
                return JsonSerializer.Deserialize<Statistics>(ref reader)!;
            case "Scan":
                return JsonSerializer.Deserialize<Scan>(ref reader)!;
            case "Shipyard":
                return JsonSerializer.Deserialize<Shipyard>(ref reader)!;
            case "Docked":
                return JsonSerializer.Deserialize<Docked>(ref reader)!;
            case "LeaveBody":
                return JsonSerializer.Deserialize<LeaveBody>(ref reader)!;
            case "Progress":
                return JsonSerializer.Deserialize<Progress>(ref reader)!;
            case "SupercruiseExit":
                return JsonSerializer.Deserialize<SupercruiseExit>(ref reader)!;
            case "EngineerProgress":
                return JsonSerializer.Deserialize<EngineerProgress>(ref reader)!;
            case "DockingRequested":
                return JsonSerializer.Deserialize<DockingRequested>(ref reader)!;
            case "NpcCrewPaidWage":
                return JsonSerializer.Deserialize<NpcCrewPaidWage>(ref reader)!;
            case "SupercruiseEntry":
                return JsonSerializer.Deserialize<SupercruiseEntry>(ref reader)!;
            case "DockingGranted":
                return JsonSerializer.Deserialize<DockingGranted>(ref reader)!;
            case "StartJump":
                return JsonSerializer.Deserialize<StartJump>(ref reader)!;
            case "FSSAllBodiesFound":
                return JsonSerializer.Deserialize<FSSAllBodiesFound>(ref reader)!;
            case "FSSBodySignals":
                return JsonSerializer.Deserialize<FSSBodySignals>(ref reader)!;
            case "Liftoff":
                return JsonSerializer.Deserialize<Liftoff>(ref reader)!;
            case "SupercruiseDestinationDrop":
                return JsonSerializer.Deserialize<SupercruiseDestinationDrop>(ref reader)!;
            case "FSDTarget":
                return JsonSerializer.Deserialize<FSDTarget>(ref reader)!;
            case "FSDJump":
                return JsonSerializer.Deserialize<FSDJump>(ref reader)!;
            case "CodexEntry":
                return JsonSerializer.Deserialize<CodexEntry>(ref reader)!;
            case "HullDamage":
                return JsonSerializer.Deserialize<HullDamage>(ref reader)!;
            case "MaterialCollected":
                return JsonSerializer.Deserialize<MaterialCollected>(ref reader)!;
            case "NavRoute":
                return JsonSerializer.Deserialize<NavRoute>(ref reader)!;
            case "NavRouteClear":
                return JsonSerializer.Deserialize<NavRouteClear>(ref reader)!;
            case "ScanBaryCentre":
                return JsonSerializer.Deserialize<ScanBaryCentre>(ref reader)!;
            case "JetConeBoost":
                return JsonSerializer.Deserialize<JetConeBoost>(ref reader)!;
            case "Shutdown":
                return JsonSerializer.Deserialize<Shutdown>(ref reader)!;
            case "FuelScoop":
                return JsonSerializer.Deserialize<FuelScoop>(ref reader)!;
            case "FSSDiscoveryScan":
                return JsonSerializer.Deserialize<FSSDiscoveryScan>(ref reader)!;
            case "ModuleInfo":
                return JsonSerializer.Deserialize<ModuleInfo>(ref reader)!;
            case "ShipTargeted":
                return JsonSerializer.Deserialize<ShipTargeted>(ref reader)!;
            case "AfmuRepairs":
                return JsonSerializer.Deserialize<AfmuRepairs>(ref reader)!;
            case "HeatWarning":
                return JsonSerializer.Deserialize<HeatWarning>(ref reader)!;
            case "ModuleBuy":
                return JsonSerializer.Deserialize<ModuleBuy>(ref reader)!;
            case "BuyDrones":
                return JsonSerializer.Deserialize<BuyDrones>(ref reader)!;
            case "ShieldState":
                return JsonSerializer.Deserialize<ShieldState>(ref reader)!;
            case "BuyAmmo":
                return JsonSerializer.Deserialize<BuyAmmo>(ref reader)!;
            case "EjectCargo":
                return JsonSerializer.Deserialize<EjectCargo>(ref reader)!;
            case "ApproachBody":
                return JsonSerializer.Deserialize<ApproachBody>(ref reader)!;
            case "DockSRV":
                return JsonSerializer.Deserialize<DockSRV>(ref reader)!;
            case "Touchdown":
                return JsonSerializer.Deserialize<Touchdown>(ref reader)!;
            case "SAASignalsFound":
                return JsonSerializer.Deserialize<SAASignalsFound>(ref reader)!;
            case "EngineerCraft":
                return JsonSerializer.Deserialize<EngineerCraft>(ref reader)!;
            case "MaterialTrade":
                return JsonSerializer.Deserialize<MaterialTrade>(ref reader)!;
            case "Repair":
                return JsonSerializer.Deserialize<Repair>(ref reader)!;
            case "RefuelAll":
                return JsonSerializer.Deserialize<RefuelAll>(ref reader)!;
            case "StoredModules":
                return JsonSerializer.Deserialize<StoredModules>(ref reader)!;
            case "Synthesis":
                return JsonSerializer.Deserialize<Synthesis>(ref reader)!;
            case "Scanned":
                return JsonSerializer.Deserialize<Scanned>(ref reader)!;
            case "SendText":
                return JsonSerializer.Deserialize<SendText>(ref reader)!;
            case "Embark":
                return JsonSerializer.Deserialize<Embark>(ref reader)!;
            case "MultiSellExplorationData":
                return JsonSerializer.Deserialize<MultiSellExplorationData>(ref reader)!;
            case "Backpack":
                return JsonSerializer.Deserialize<BackpackMaterials>(ref reader)!;
            case "ModuleSell":
                return JsonSerializer.Deserialize<ModuleSell>(ref reader)!;
            case "Undocked":
                return JsonSerializer.Deserialize<Undocked>(ref reader)!;
            case "RepairAll":
                return JsonSerializer.Deserialize<RepairAll>(ref reader)!;
            case "Outfitting":
                return JsonSerializer.Deserialize<Outfitting>(ref reader)!;
            case "PowerplaySalary":
                return JsonSerializer.Deserialize<PowerplaySalary>(ref reader)!;
            case "RedeemVoucher":
                return JsonSerializer.Deserialize<RedeemVoucher>(ref reader)!;
            case "SAAScanComplete":
                return JsonSerializer.Deserialize<SAAScanComplete>(ref reader)!;
            case "Friends":
                return JsonSerializer.Deserialize<Friends>(ref reader)!;
            case "LaunchSRV":
                return JsonSerializer.Deserialize<LaunchSRV>(ref reader)!;
            case "SuitLoadout":
                return JsonSerializer.Deserialize<SuitLoadout>(ref reader)!;
            case "Disembark":
                return JsonSerializer.Deserialize<Disembark>(ref reader)!;
            case "MaterialDiscovered":
                return JsonSerializer.Deserialize<MaterialDiscovered>(ref reader)!;
            case "StoredShips":
                return JsonSerializer.Deserialize<StoredShips>(ref reader)!;
            case "ScanOrganic":
                return JsonSerializer.Deserialize<ScanOrganic>(ref reader)!;
            case "Market":
                return JsonSerializer.Deserialize<Market>(ref reader)!;
            case "MissionCompleted":
                return JsonSerializer.Deserialize<MissionCompleted>(ref reader)!;
            case "SellShipOnRebuy":
                return JsonSerializer.Deserialize<SellShipOnRebuy>(ref reader)!;
            case "MissionAccepted":
                return JsonSerializer.Deserialize<MissionAccepted>(ref reader)!;
            case "ApproachSettlement":
                return JsonSerializer.Deserialize<ApproachSettlement>(ref reader)!;
            case "Screenshot":
                return JsonSerializer.Deserialize<Screenshot>(ref reader)!;
            case "ModuleSwap":
                return JsonSerializer.Deserialize<ModuleSwap>(ref reader)!;
            case "UnderAttack":
                return JsonSerializer.Deserialize<UnderAttack>(ref reader)!;
            case "DataScanned":
                return JsonSerializer.Deserialize<DataScanned>(ref reader)!;
            case "DockingDenied":
                return JsonSerializer.Deserialize<DockingDenied>(ref reader)!;
            case "FetchRemoteModule":
                return JsonSerializer.Deserialize<FetchRemoteModule>(ref reader)!; 
            case "EngineerContribution":
                return JsonSerializer.Deserialize<EngineerContribution>(ref reader)!;
            case "CollectCargo":
                return JsonSerializer.Deserialize<CollectCargo>(ref reader)!;
            case "ModuleRetrieve":
                return JsonSerializer.Deserialize<ModuleRetrieve>(ref reader)!;
            case "MarketBuy":
                return JsonSerializer.Deserialize<MarketBuy>(ref reader)!;
            case "SellDrones":
                return JsonSerializer.Deserialize<SellDrones>(ref reader)!;
            case "Interdicted":
                return JsonSerializer.Deserialize<Interdicted>(ref reader)!;
            case "SellOrganicData":
                return JsonSerializer.Deserialize<SellOrganicData>(ref reader)!;
            case "WingAdd":
                return JsonSerializer.Deserialize<WingAdd>(ref reader)!;
            case "WingInvite":
                return JsonSerializer.Deserialize<WingInvite>(ref reader)!;
            case "WingJoin":
                return JsonSerializer.Deserialize<WingJoin>(ref reader)!;
            case "WingLeave":
                return JsonSerializer.Deserialize<WingLeave>(ref reader)!;
            case "Bounty":
                return JsonSerializer.Deserialize<Bounty>(ref reader)!;
            case "CommitCrime":
                return JsonSerializer.Deserialize<CommitCrime>(ref reader)!;
            case "ModuleStore":
                return JsonSerializer.Deserialize<ModuleStore>(ref reader)!;
            case "FactionKillBond":
                return JsonSerializer.Deserialize<FactionKillBond>(ref reader)!;
            case "RebootRepair":
                return JsonSerializer.Deserialize<RebootRepair>(ref reader)!;
            case "LaunchDrone":
                return JsonSerializer.Deserialize<LaunchDrone>(ref reader)!;
            case "SellMicroResources":
                return JsonSerializer.Deserialize<SellMicroResources>(ref reader)!;
            case "NavBeaconScan":
                return JsonSerializer.Deserialize<NavBeaconScan>(ref reader)!;
            case "SearchAndRescue":
                return JsonSerializer.Deserialize<SearchAndRescue>(ref reader)!;
            case "MarketSell":
                return JsonSerializer.Deserialize<MarketSell>(ref reader)!;
            default:
                logger.LogWarning("Unknown Journal event type {EventName}", eventName);
                return JsonSerializer.Deserialize<JournalBase>(ref reader)!;
        }
    }

    public override void Write(Utf8JsonWriter writer, JournalBase value, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }
}