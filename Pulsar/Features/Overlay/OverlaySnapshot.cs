namespace Pulsar.Features.Overlay;

public sealed class OverlaySnapshot
{
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public string? Commander { get; set; }

    public string? Ship { get; set; }

    public string? ShipName { get; set; }

    public string? ShipIdent { get; set; }

    public string? System { get; set; }

    public string? Body { get; set; }

    public string? Station { get; set; }

    public string? StationType { get; set; }

    public string? Destination { get; set; }

    public int Cargo { get; set; }

    public decimal? FuelMain { get; set; }

    public decimal? FuelReservoir { get; set; }

    public decimal? FuelCapacity { get; set; }

    public decimal FuelPercent { get; set; }

    public bool LowFuel { get; set; }

    public bool FuelScooping { get; set; }

    public int? FuelSecondsRemaining { get; set; }

    public List<string> Alerts { get; set; } = [];

    public string? DockingState { get; set; }

    public string? DockingStation { get; set; }

    public int? LandingPad { get; set; }

    public string? CurrentSystemScan { get; set; }

    public int BodiesScanned { get; set; }

    public int TotalBodies { get; set; }

    public long EstimatedSystemValue { get; set; }

    public List<OverlayBodyTarget> HighValueBodies { get; set; } = [];

    public string? BiologySpecies { get; set; }

    public string? BiologyGenus { get; set; }

    public int? BiologySample { get; set; }

    public int? BiologyRequiredDistance { get; set; }
}

public sealed class OverlayBodyTarget
{
    public string? Name { get; set; }

    public string? Class { get; set; }

    public decimal DistanceFromArrivalLs { get; set; }

    public long EstimatedValue { get; set; }

    public bool Terraformable { get; set; }
}
