namespace Observatory.Framework.Files;

using Journal;
using ParameterTypes;

/// <summary>
///     Elite Dangerous navroute.json file. Contains data about currently plotted FSD jump route.
/// </summary>
public class NavRouteFile : JournalBase
{
    /// <summary>
    ///     List of star systems and their locations in the current route.
    /// </summary>
    public List<Route> Route { get; init; }
}