namespace Observatory.Framework.Files.Journal.Odyssey;

/// <summary>
///     Written when the player upgrades a hand weapon.
/// </summary>
public class UpgradeWeapon : JournalBase
{
    public string Name { get; init; }
    public string Name_Localised { get; init; }
    public ulong SuitModuleID { get; init; }
    public int Class { get; init; }
    public int Cost { get; init; }
}