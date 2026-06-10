namespace Pulsar.Features.Overlay;

using Observatory.Framework.Files.Journal.Exploration;

internal static class ExplorationValueCalculator
{
    private const double MinimumBodyValue = 500;
    private const double OdysseyMinimumMappingBonus = 555;

    public static ExplorationValue Calculate(Scan body, bool odysseyBonus = true)
    {
        var firstDiscoverer = !body.WasDiscovered;

        if (!string.IsNullOrWhiteSpace(body.StarType))
        {
            var k = GetStarClassK(body.StarType);
            var starScan = k + body.StellarMass * k / 66.25d;
            var starHonk = starScan / 3d;

            if (firstDiscoverer)
            {
                starScan *= 2.6d;
                starHonk *= 2.6d;
            }

            return new ExplorationValue(Round(starScan), 0, Round(starHonk), Round(starScan), 0, Round(starHonk));
        }

        var (baseValue, terraformBonus, terraformMultiplier) = GetPlanetClassK(
            body.PlanetClass,
            IsTerraformable(body));
        var mass = Math.Max(body.MassEM, 0.000001d);
        const double q = 0.56591828d;
        var finalK = baseValue + terraformBonus;
        var finalMinK = baseValue + terraformBonus * terraformMultiplier;
        var firstMapper = !body.WasMapped;
        var mappingMultiplier = firstDiscoverer && firstMapper
            ? 3.699622554d
            : firstMapper
                ? 8.0956d
                : 10d / 3d;

        var scan = finalK + finalK * q * Math.Pow(mass, 0.2d);
        var minScan = finalMinK + finalMinK * q * Math.Pow(mass, 0.2d);
        var mapped = scan * mappingMultiplier;
        var minMapped = minScan * mappingMultiplier;
        var honk = scan / 3d;
        var minHonk = minScan / 3d;

        if (odysseyBonus)
        {
            mapped += Math.Max(mapped * 0.3d, OdysseyMinimumMappingBonus);
            minMapped += Math.Max(minMapped * 0.3d, OdysseyMinimumMappingBonus);
        }

        scan = Clamp(scan);
        minScan = Clamp(minScan);
        mapped = Clamp(mapped);
        minMapped = Clamp(minMapped);
        honk = Clamp(honk);
        minHonk = Clamp(minHonk);

        if (firstDiscoverer)
        {
            scan *= 2.6d;
            minScan *= 2.6d;
            mapped *= 2.6d;
            minMapped *= 2.6d;
            honk *= 2.6d;
            minHonk *= 2.6d;
        }

        return new ExplorationValue(Round(scan), Round(mapped), Round(honk), Round(minScan), Round(minMapped), Round(minHonk));
    }

    public static bool IsTerraformable(Scan body)
    {
        return !string.IsNullOrWhiteSpace(body.TerraformState);
    }

    private static double GetStarClassK(string starClass)
    {
        if (starClass is "N" or "H")
            return 22628;

        return starClass.StartsWith('D') ? 14057 : 1200;
    }

    private static (double BaseValue, double TerraformBonus, double TerraformMultiplier) GetPlanetClassK(
        string? planetClass,
        bool terraformable)
    {
        return planetClass switch
        {
            "Metal rich body" => (21790, 0, 1),
            "Ammonia world" => (96932, 0, 1),
            "Sudarsky class I gas giant" or "Class I gas giant" => (1656, 0, 1),
            "Sudarsky class II gas giant" or "Class II gas giant" or "High metal content body" =>
                terraformable ? (9654, 100677, 0.9d) : (9654, 0, 1),
            "Water world" => terraformable ? (64831, 116295, 0.75d) : (64831, 0, 1),
            "Earthlike body" or "Earthlike world" => (64831, 116295, terraformable ? 0 : 1),
            _ => terraformable ? (300, 93328, 0.9d) : (300, 0, 1)
        };
    }

    private static double Clamp(double value)
    {
        return value > MinimumBodyValue ? value : MinimumBodyValue;
    }

    private static long Round(double value)
    {
        return Convert.ToInt64(Math.Round(value, MidpointRounding.AwayFromZero));
    }
}

internal sealed record ExplorationValue(
    long Scan,
    long Mapped,
    long Honk,
    long MinScan,
    long MinMapped,
    long MinHonk);
