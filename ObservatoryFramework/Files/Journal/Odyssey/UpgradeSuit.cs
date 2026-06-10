namespace Observatory.Framework.Files.Journal.Odyssey;

using ParameterTypes;

/// <summary>
///     Written when the player upgrades their flight suit.
/// </summary>
public class UpgradeSuit : JournalBase
{
    public string Name { get; init; }
    public string Name_Localised { get; init; }
    public ulong SuitID { get; init; }
    public int Class { get; init; }
    public int Cost { get; init; }
    public Material[] Resources { get; init; }
}