namespace Observatory.Framework.Files.Journal.Odyssey;

using ParameterTypes;

/// <summary>
///     Written when the player creates a new suit loadout.
/// </summary>
public class CreateSuitLoadout : DeleteSuitLoadout
{
    public List<SuitModule> Modules { get; init; }
    public IList<string> SuitMods { get; init; }
}