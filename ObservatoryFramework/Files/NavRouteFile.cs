using System.Collections.Immutable;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files;

/// <summary>
/// Elite Dangerous navroute.json file. Contains data about currently plotted FSD jump route.
/// </summary>
public class NavRouteFile : JournalBase
{
    public override string Event => "Route";
    /// <summary>
    /// List of star systems and their locations in the current route.
    /// </summary>
    public ImmutableList<Route> Route { get; init; }
}