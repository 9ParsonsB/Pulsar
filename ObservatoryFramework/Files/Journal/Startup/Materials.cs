namespace Observatory.Framework.Files.Journal.Startup;

using ParameterTypes;

/// <summary>
///     Written at startup, when loading from main menu into game.
/// </summary>
public class Materials : JournalBase
{
    public virtual List<Material> Raw { get; init; }
    public virtual List<Material> Manufactured { get; init; }
    public virtual List<Material> Encoded { get; init; }
}