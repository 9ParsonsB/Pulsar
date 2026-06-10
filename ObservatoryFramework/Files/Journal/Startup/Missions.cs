namespace Observatory.Framework.Files.Journal.Startup;

using ParameterTypes;

/// <summary>
///     Written at startup.
/// </summary>
public class Missions : JournalBase
{
    public List<Mission> Active { get; init; }
    public List<Mission> Failed { get; init; }
    public List<Mission> Complete { get; init; }
}