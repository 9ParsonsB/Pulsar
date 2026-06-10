namespace Pulsar.Features.Overlay;

using Observatory.Framework.Files;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Files.Journal.Combat;
using Observatory.Framework.Files.Journal.Exploration;
using Observatory.Framework.Files.Journal.Odyssey;
using Observatory.Framework.Files.Journal.Other;
using Observatory.Framework.Files.Journal.Startup;
using Observatory.Framework.Files.Journal.StationServices;
using Observatory.Framework.Files.Journal.Travel;
using Observatory.Framework.Files.ParameterTypes;

public interface IOverlayStateService
{
    OverlaySnapshot GetSnapshot();

    void ApplyStatus(Status status);

    void ApplyJournals(IEnumerable<JournalBase> journals);
}

public class OverlayStateService : IOverlayStateService
{
    private const long ValuableBodyMinimum = 400_000;
    private readonly Lock gate = new();
    private readonly OverlaySnapshot snapshot = new();
    private readonly Dictionary<int, OverlayBodyTarget> scannedBodies = [];
    private readonly Queue<decimal> recentFuelDeltas = [];
    private decimal? lastFuelMain;

    private static readonly IReadOnlyDictionary<string, int> ColonyDistancesByGenus = new Dictionary<string, int>
    {
        { "Aleoida", 150 },
        { "Bacterium", 500 },
        { "Cactoida", 300 },
        { "Clypeus", 150 },
        { "Concha", 150 },
        { "Electricae", 1000 },
        { "Fonticulua", 500 },
        { "Frutexa", 150 },
        { "Fumerola", 100 },
        { "Fungoida", 300 },
        { "Osseus", 800 },
        { "Recepta", 150 },
        { "Stratum", 500 },
        { "Tubus", 800 },
        { "Tussock", 200 }
    };

    private static readonly IReadOnlyDictionary<string, string> EnglishGenusByIdentifier = new Dictionary<string, string>
    {
        { "$Codex_Ent_Aleoids_Genus_Name;", "Aleoida" },
        { "$Codex_Ent_Bacterial_Genus_Name;", "Bacterium" },
        { "$Codex_Ent_Cactoid_Genus_Name;", "Cactoida" },
        { "$Codex_Ent_Clepeus_Genus_Name;;", "Clypeus" },
        { "$Codex_Ent_Clypeus_Genus_Name;", "Clypeus" },
        { "$Codex_Ent_Conchas_Genus_Name;", "Concha" },
        { "$Codex_Ent_Electricae_Genus_Name;", "Electricae" },
        { "$Codex_Ent_Fonticulus_Genus_Name;", "Fonticulua" },
        { "$Codex_Ent_Shrubs_Genus_Name;", "Frutexa" },
        { "$Codex_Ent_Fumerolas_Genus_Name;", "Fumerola" },
        { "$Codex_Ent_Fungoids_Genus_Name;", "Fungoida" },
        { "$Codex_Ent_Osseus_Genus_Name;", "Osseus" },
        { "$Codex_Ent_Recepta_Genus_Name;", "Recepta" },
        { "$Codex_Ent_Stratum_Genus_Name;", "Stratum" },
        { "$Codex_Ent_Tubus_Genus_Name;", "Tubus" },
        { "$Codex_Ent_Tussocks_Genus_Name;", "Tussock" }
    };

    public OverlaySnapshot GetSnapshot()
    {
        lock (gate)
        {
            return new OverlaySnapshot
            {
                UpdatedAt = snapshot.UpdatedAt,
                Commander = snapshot.Commander,
                Ship = snapshot.Ship,
                ShipName = snapshot.ShipName,
                ShipIdent = snapshot.ShipIdent,
                System = snapshot.System,
                Body = snapshot.Body,
                Station = snapshot.Station,
                StationType = snapshot.StationType,
                Destination = snapshot.Destination,
                Cargo = snapshot.Cargo,
                FuelMain = snapshot.FuelMain,
                FuelReservoir = snapshot.FuelReservoir,
                FuelCapacity = snapshot.FuelCapacity,
                FuelPercent = snapshot.FuelPercent,
                LowFuel = snapshot.LowFuel,
                FuelScooping = snapshot.FuelScooping,
                FuelSecondsRemaining = snapshot.FuelSecondsRemaining,
                Alerts = [..snapshot.Alerts],
                DockingState = snapshot.DockingState,
                DockingStation = snapshot.DockingStation,
                LandingPad = snapshot.LandingPad,
                CurrentSystemScan = snapshot.CurrentSystemScan,
                BodiesScanned = snapshot.BodiesScanned,
                TotalBodies = snapshot.TotalBodies,
                EstimatedSystemValue = snapshot.EstimatedSystemValue,
                HighValueBodies = [..snapshot.HighValueBodies.Select(CloneBody)],
                BiologySpecies = snapshot.BiologySpecies,
                BiologyGenus = snapshot.BiologyGenus,
                BiologySample = snapshot.BiologySample,
                BiologyRequiredDistance = snapshot.BiologyRequiredDistance
            };
        }
    }

    public void ApplyStatus(Status status)
    {
        lock (gate)
        {
            snapshot.UpdatedAt = DateTimeOffset.UtcNow;
            snapshot.Cargo = status.Cargo.HasValue ? (int)Math.Round(status.Cargo.Value) : snapshot.Cargo;
            snapshot.Destination = status.Destination?.Name ?? snapshot.Destination;

            if (status.Fuel != null)
            {
                snapshot.FuelMain = Convert.ToDecimal(status.Fuel.FuelMain);
                snapshot.FuelReservoir = Convert.ToDecimal(status.Fuel.FuelReservoir);
                UpdateFuelTrend(snapshot.FuelMain);
            }

            var capacity = snapshot.FuelCapacity.GetValueOrDefault();
            if (capacity > 0 && snapshot.FuelMain.HasValue)
                snapshot.FuelPercent = Math.Round(snapshot.FuelMain.Value / capacity * 100m, 1);

            snapshot.LowFuel = snapshot.FuelPercent is > 0 and <= 25;
            snapshot.FuelScooping = status.Flags.ToString().Contains("FuelScoop", StringComparison.OrdinalIgnoreCase);
            snapshot.FuelSecondsRemaining = CalculateFuelSecondsRemaining();
        }
    }

    public void ApplyJournals(IEnumerable<JournalBase> journals)
    {
        lock (gate)
        {
            foreach (var journal in journals.OrderBy(j => j.Timestamp))
            {
                snapshot.UpdatedAt = DateTimeOffset.UtcNow;

                switch (journal)
                {
                    case LoadGame loadGame:
                        snapshot.Commander = loadGame.Commander;
                        snapshot.Ship = loadGame.Ship_Localised ?? loadGame.Ship;
                        snapshot.ShipName = loadGame.ShipName;
                        snapshot.ShipIdent = loadGame.ShipIdent;
                        snapshot.FuelCapacity = Convert.ToDecimal(loadGame.FuelCapacity);
                        snapshot.FuelMain = Convert.ToDecimal(loadGame.FuelLevel);
                        UpdateFuelTrend(snapshot.FuelMain, true);
                        if (snapshot.FuelCapacity > 0)
                            snapshot.FuelPercent = Math.Round(snapshot.FuelMain.GetValueOrDefault() /
                                                              snapshot.FuelCapacity.Value * 100m, 1);
                        snapshot.LowFuel = snapshot.FuelPercent is > 0 and <= 25;
                        snapshot.FuelSecondsRemaining = CalculateFuelSecondsRemaining();
                        break;
                    case Cargo cargo:
                        snapshot.Cargo = cargo.Count;
                        break;
                    case ReservoirReplenished replenished:
                        snapshot.FuelMain = Convert.ToDecimal(replenished.FuelMain);
                        snapshot.FuelReservoir = Convert.ToDecimal(replenished.FuelReservoir);
                        UpdateFuelTrend(snapshot.FuelMain);
                        if (snapshot.FuelCapacity > 0)
                            snapshot.FuelPercent = Math.Round(snapshot.FuelMain.GetValueOrDefault() /
                                                              snapshot.FuelCapacity.Value * 100m, 1);
                        snapshot.LowFuel = snapshot.FuelPercent is > 0 and <= 25;
                        snapshot.FuelSecondsRemaining = CalculateFuelSecondsRemaining();
                        break;
                    case Location location:
                        snapshot.System = location.StarSystem;
                        snapshot.Body = location.Body;
                        snapshot.Station = location.Docked ? location.StationName : null;
                        snapshot.StationType = location.StationType;
                        break;
                    case FSDJump fsdJump:
                        snapshot.System = fsdJump.StarSystem;
                        snapshot.Body = fsdJump.Body;
                        snapshot.Station = null;
                        snapshot.StationType = null;
                        ResetSystemScan(fsdJump.StarSystem);
                        break;
                    case Docked docked:
                        snapshot.Station = docked.StationName;
                        snapshot.StationType = docked.StationType;
                        snapshot.System ??= docked.StarSystem;
                        snapshot.DockingState = "Docked";
                        snapshot.DockingStation = docked.StationName;
                        snapshot.LandingPad = null;
                        break;
                    case Undocked:
                        snapshot.Station = null;
                        snapshot.StationType = null;
                        snapshot.DockingState = null;
                        snapshot.DockingStation = null;
                        snapshot.LandingPad = null;
                        break;
                    case DockingGranted dockingGranted:
                        snapshot.DockingState = "Granted";
                        snapshot.DockingStation = dockingGranted.StationName;
                        snapshot.LandingPad = dockingGranted.LandingPad;
                        break;
                    case DockingDenied dockingDenied:
                        snapshot.DockingState = $"Denied: {dockingDenied.Reason}";
                        snapshot.DockingStation = dockingDenied.StationName;
                        snapshot.LandingPad = null;
                        break;
                    case DockingCancelled dockingCancelled:
                        snapshot.DockingState = "Cancelled";
                        snapshot.DockingStation = dockingCancelled.StationName;
                        snapshot.LandingPad = null;
                        break;
                    case DockingRequested dockingRequested:
                        snapshot.DockingState = "Requesting";
                        snapshot.DockingStation = dockingRequested.StationName;
                        snapshot.LandingPad = null;
                        break;
                    case Liftoff:
                        break;
                    case Touchdown touchdown:
                        snapshot.Body = touchdown.Body;
                        break;
                    case UnderAttack:
                        PushAlert("Under attack");
                        break;
                    case HullDamage hullDamage:
                        PushAlert($"Hull damage {Math.Round(hullDamage.Health)}%");
                        break;
                    case FSSDiscoveryScan discovery:
                        if (!string.Equals(snapshot.CurrentSystemScan, discovery.SystemName, StringComparison.Ordinal))
                            ResetSystemScan(discovery.SystemName);
                        snapshot.TotalBodies = discovery.BodyCount;
                        break;
                    case Scan scan:
                        ApplyScan(scan);
                        break;
                    case ScanOrganic scanOrganic:
                        ApplyOrganicScan(scanOrganic);
                        break;
                }
            }
        }
    }

    private void ApplyScan(Scan scan)
    {
        if (!string.Equals(snapshot.CurrentSystemScan, scan.StarSystem, StringComparison.Ordinal))
            ResetSystemScan(scan.StarSystem);

        var value = ExplorationValueCalculator.Calculate(scan);
        scannedBodies[scan.BodyID] = new OverlayBodyTarget
        {
            Name = scan.BodyName,
            Class = scan.StarType ?? scan.PlanetClass,
            DistanceFromArrivalLs = Convert.ToDecimal(scan.DistanceFromArrivalLS),
            EstimatedValue = value.Mapped > 0 ? value.Mapped : value.Scan,
            Terraformable = ExplorationValueCalculator.IsTerraformable(scan)
        };

        snapshot.BodiesScanned = scannedBodies.Count;
        snapshot.EstimatedSystemValue = scannedBodies.Values.Sum(body => body.EstimatedValue);
        snapshot.HighValueBodies = scannedBodies.Values
            .Where(body => body.EstimatedValue >= ValuableBodyMinimum || body.Terraformable)
            .OrderByDescending(body => body.EstimatedValue)
            .Take(5)
            .Select(CloneBody)
            .ToList();
    }

    private void ResetSystemScan(string? systemName)
    {
        snapshot.CurrentSystemScan = systemName;
        snapshot.System = systemName ?? snapshot.System;
        snapshot.TotalBodies = 0;
        snapshot.BodiesScanned = 0;
        snapshot.EstimatedSystemValue = 0;
        snapshot.HighValueBodies = [];
        scannedBodies.Clear();
    }

    private void PushAlert(string message)
    {
        snapshot.Alerts.Insert(0, message);
        if (snapshot.Alerts.Count > 4)
            snapshot.Alerts.RemoveRange(4, snapshot.Alerts.Count - 4);
    }

    private static OverlayBodyTarget CloneBody(OverlayBodyTarget body)
    {
        return new OverlayBodyTarget
        {
            Name = body.Name,
            Class = body.Class,
            DistanceFromArrivalLs = body.DistanceFromArrivalLs,
            EstimatedValue = body.EstimatedValue,
            Terraformable = body.Terraformable
        };
    }

    private void ApplyOrganicScan(ScanOrganic scanOrganic)
    {
        snapshot.BiologySpecies = scanOrganic.Species_Localised;
        snapshot.BiologyGenus = EnglishGenusByIdentifier.GetValueOrDefault(scanOrganic.Genus, scanOrganic.Genus_Localised);
        snapshot.BiologyRequiredDistance = ColonyDistancesByGenus.GetValueOrDefault(snapshot.BiologyGenus ?? string.Empty, 100);

        snapshot.BiologySample = scanOrganic.ScanType switch
        {
            ScanOrganicType.Log => 1,
            ScanOrganicType.Sample => 2,
            ScanOrganicType.Analyse => 3,
            _ => snapshot.BiologySample
        };

        if (scanOrganic.ScanType == ScanOrganicType.Analyse)
        {
            snapshot.BiologySpecies = null;
            snapshot.BiologyGenus = null;
            snapshot.BiologySample = null;
            snapshot.BiologyRequiredDistance = null;
        }
    }

    private void UpdateFuelTrend(decimal? fuelMain, bool reset = false)
    {
        if (reset)
        {
            recentFuelDeltas.Clear();
            lastFuelMain = fuelMain;
            return;
        }

        if (!fuelMain.HasValue)
            return;

        if (lastFuelMain.HasValue)
        {
            var delta = fuelMain.Value - lastFuelMain.Value;
            if (delta != 0)
            {
                recentFuelDeltas.Enqueue(delta);
                while (recentFuelDeltas.Count > 4)
                    recentFuelDeltas.Dequeue();
            }
        }

        lastFuelMain = fuelMain;
    }

    private int? CalculateFuelSecondsRemaining()
    {
        if (!snapshot.FuelMain.HasValue || recentFuelDeltas.Count == 0)
            return null;

        var average = recentFuelDeltas.Average();
        if (average == 0)
            return null;

        if (average < 0)
            return (int)Math.Round(snapshot.FuelMain.Value / Math.Abs(average));

        if (!snapshot.FuelCapacity.HasValue)
            return null;

        var missing = snapshot.FuelCapacity.Value - snapshot.FuelMain.Value;
        if (missing <= 0)
            return 0;

        return (int)Math.Round(missing / average);
    }
}
