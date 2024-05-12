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
public class JournalConverter(ILogger logger) : JsonConverter<JournalBase>
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
                case JsonTokenType.None:
                    break;
                case JsonTokenType.StartObject:
                    state = JournalReaderState.Start;
                    break;
                case JsonTokenType.EndObject:
                    state = JournalReaderState.End;
                    break;
                case JsonTokenType.StartArray:
                    break;
                case JsonTokenType.EndArray:
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
                case JsonTokenType.String:
                    break;
                case JsonTokenType.Number:
                    break;
                case JsonTokenType.True:
                    break;
                case JsonTokenType.False:
                    break;
                case JsonTokenType.Null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        } while (clone.Read());

        return new() { Timestamp = timestamp!.Value, Event = eventName! };

        // TODO: handle inf (invalid data) in the journal files
        // else if (typeof(TJournal) == typeof(Scan) && json.Contains("\"RotationPeriod\":inf"))
        // {
        //     deserialized = JsonSerializer.Deserialize<TJournal>(json.Replace("\"RotationPeriod\":inf,", ""));
        // }
    }

    private JournalBase GetDestinationType(ref Utf8JsonReader reader, string eventName)
    {
        switch (eventName.ToLower())
        {
            case "fileheader":
                return JsonSerializer.Deserialize<FileHeader>(ref reader)!;
            case "commander":
                return JsonSerializer.Deserialize<Commander>(ref reader)!;
            case "materials":
                return JsonSerializer.Deserialize<Materials>(ref reader)!;
            case "rank":
                return JsonSerializer.Deserialize<Rank>(ref reader)!;
            case "music":
                return JsonSerializer.Deserialize<Music>(ref reader)!;
            case "cargo":
                return JsonSerializer.Deserialize<Cargo>(ref reader)!;
            case "loadout":
                return JsonSerializer.Deserialize<Loadout>(ref reader)!;
            case "missions":
                return JsonSerializer.Deserialize<Missions>(ref reader)!;
            case "fsssignaldiscovered":
                return JsonSerializer.Deserialize<FSSSignalDiscovered>(ref reader)!;
            case "reputation":
                return JsonSerializer.Deserialize<Reputation>(ref reader)!;
            case "loadgame":
                return JsonSerializer.Deserialize<LoadGame>(ref reader)!;
            case "receivetext":
                return JsonSerializer.Deserialize<ReceiveText>(ref reader)!;
            case "shiplocker":
                return JsonSerializer.Deserialize<ShipLockerMaterials>(ref reader)!;
            case "location":
                return JsonSerializer.Deserialize<Location>(ref reader)!;
            case "powerplay":
                return JsonSerializer.Deserialize<Powerplay>(ref reader)!;
            case "reservoirreplenished":
                return JsonSerializer.Deserialize<ReservoirReplenished>(ref reader)!;
            case "statistics":
                return JsonSerializer.Deserialize<Statistics>(ref reader)!;
            case "scan":
                return JsonSerializer.Deserialize<Scan>(ref reader)!;
            case "shipyard":
                return JsonSerializer.Deserialize<Shipyard>(ref reader)!;
            case "docked":
                return JsonSerializer.Deserialize<Docked>(ref reader)!;
            case "leavebody":
                return JsonSerializer.Deserialize<LeaveBody>(ref reader)!;
            case "progress":
                return JsonSerializer.Deserialize<Progress>(ref reader)!;
            case "supercruiseexit":
                return JsonSerializer.Deserialize<SupercruiseExit>(ref reader)!;
            case "engineerprogress":
                return JsonSerializer.Deserialize<EngineerProgress>(ref reader)!;
            case "dockingrequested":
                return JsonSerializer.Deserialize<DockingRequested>(ref reader)!;
            case "npccrewpaidwage":
                return JsonSerializer.Deserialize<NpcCrewPaidWage>(ref reader)!;
            case "supercruiseentry":
                return JsonSerializer.Deserialize<SupercruiseEntry>(ref reader)!;
            case "dockinggranted":
                return JsonSerializer.Deserialize<DockingGranted>(ref reader)!;
            case "startjump":
                return JsonSerializer.Deserialize<StartJump>(ref reader)!;
            case "fssallbodiesfound":
                return JsonSerializer.Deserialize<FSSAllBodiesFound>(ref reader)!;
            case "fssbodysignals":
                return JsonSerializer.Deserialize<FSSBodySignals>(ref reader)!;
            case "liftoff":
                return JsonSerializer.Deserialize<Liftoff>(ref reader)!;
            case "supercruisedestinationdrop":
                return JsonSerializer.Deserialize<SupercruiseDestinationDrop>(ref reader)!;
            case "fsdtarget":
                return JsonSerializer.Deserialize<FSDTarget>(ref reader)!;
            case "fsdjump":
                return JsonSerializer.Deserialize<FSDJump>(ref reader)!;
            case "codexentry":
                return JsonSerializer.Deserialize<CodexEntry>(ref reader)!;
            case "hulldamage":
                return JsonSerializer.Deserialize<HullDamage>(ref reader)!;
            case "materialcollected":
                return JsonSerializer.Deserialize<MaterialCollected>(ref reader)!;
            case "navroute":
                return JsonSerializer.Deserialize<NavRoute>(ref reader)!;
            case "navrouteclear":
                return JsonSerializer.Deserialize<NavRouteClear>(ref reader)!;
            case "scanbarycentre":
                return JsonSerializer.Deserialize<ScanBaryCentre>(ref reader)!;
            case "jetconeboost":
                return JsonSerializer.Deserialize<JetConeBoost>(ref reader)!;
            case "shutdown":
                return JsonSerializer.Deserialize<Shutdown>(ref reader)!;
            case "fuelscoop":
                return JsonSerializer.Deserialize<FuelScoop>(ref reader)!;
            case "fssdiscoveryscan":
                return JsonSerializer.Deserialize<FSSDiscoveryScan>(ref reader)!;
            case "moduleinfo":
                return JsonSerializer.Deserialize<ModuleInfo>(ref reader)!;
            case "shiptargeted":
                return JsonSerializer.Deserialize<ShipTargeted>(ref reader)!;
            case "afmurepairs":
                return JsonSerializer.Deserialize<AfmuRepairs>(ref reader)!;
            case "heatwarning":
                return JsonSerializer.Deserialize<HeatWarning>(ref reader)!;
            case "modulebuy":
                return JsonSerializer.Deserialize<ModuleBuy>(ref reader)!;
            case "buydrones":
                return JsonSerializer.Deserialize<BuyDrones>(ref reader)!;
            case "shieldstate":
                return JsonSerializer.Deserialize<ShieldState>(ref reader)!;
            case "buyammo":
                return JsonSerializer.Deserialize<BuyAmmo>(ref reader)!;
            case "ejectcargo":
                return JsonSerializer.Deserialize<EjectCargo>(ref reader)!;
            case "approachbody":
                return JsonSerializer.Deserialize<ApproachBody>(ref reader)!;
            case "docksrv":
                return JsonSerializer.Deserialize<DockSRV>(ref reader)!;
            case "touchdown":
                return JsonSerializer.Deserialize<Touchdown>(ref reader)!;
            case "saasignalsfound":
                return JsonSerializer.Deserialize<SAASignalsFound>(ref reader)!;
            case "engineercraft":
                return JsonSerializer.Deserialize<EngineerCraft>(ref reader)!;
            case "materialtrade":
                return JsonSerializer.Deserialize<MaterialTrade>(ref reader)!;
            case "repair":
                return JsonSerializer.Deserialize<Repair>(ref reader)!;
            case "refuelall":
                return JsonSerializer.Deserialize<RefuelAll>(ref reader)!;
            case "storedmodules":
                return JsonSerializer.Deserialize<StoredModules>(ref reader)!;
            case "synthesis":
                return JsonSerializer.Deserialize<Synthesis>(ref reader)!;
            case "scanned":
                return JsonSerializer.Deserialize<Scanned>(ref reader)!;
            case "sendtext":
                return JsonSerializer.Deserialize<SendText>(ref reader)!;
            case "embark":
                return JsonSerializer.Deserialize<Embark>(ref reader)!;
            case "multisellexplorationdata":
                return JsonSerializer.Deserialize<MultiSellExplorationData>(ref reader)!;
            case "backpack":
                return JsonSerializer.Deserialize<BackpackMaterials>(ref reader)!;
            case "modulesell":
                return JsonSerializer.Deserialize<ModuleSell>(ref reader)!;
            case "undocked":
                return JsonSerializer.Deserialize<Undocked>(ref reader)!;
            case "repairall":
                return JsonSerializer.Deserialize<RepairAll>(ref reader)!;
            case "outfitting":
                return JsonSerializer.Deserialize<Outfitting>(ref reader)!;
            case "powerplaysalary":
                return JsonSerializer.Deserialize<PowerplaySalary>(ref reader)!;
            case "redeemvoucher":
                return JsonSerializer.Deserialize<RedeemVoucher>(ref reader)!;
            case "saascancomplete":
                return JsonSerializer.Deserialize<SAAScanComplete>(ref reader)!;
            case "friends":
                return JsonSerializer.Deserialize<Friends>(ref reader)!;
            case "launchsrv":
                return JsonSerializer.Deserialize<LaunchSRV>(ref reader)!;
            case "suitloadout":
                return JsonSerializer.Deserialize<SuitLoadout>(ref reader)!;
            case "disembark":
                return JsonSerializer.Deserialize<Disembark>(ref reader)!;
            case "materialdiscovered":
                return JsonSerializer.Deserialize<MaterialDiscovered>(ref reader)!;
            case "storedships":
                return JsonSerializer.Deserialize<StoredShips>(ref reader)!;
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